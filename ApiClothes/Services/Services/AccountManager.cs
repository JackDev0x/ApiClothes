using ApiClothes.Entities;
using ApiClothes.RequestsModels;
using ApiClothes.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using ApiClothes.DtoModels;
using Azure.Storage.Blobs;

namespace ApiClothes.Services.Services
{
    public class AccountManager : IAccountManager
    {
        private readonly PlatformDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;


        public AccountManager(PlatformDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;

            var connectionString = _configuration.GetValue<string>("AZURE_STORAGE_CONNECTION_STRING");
            _blobServiceClient = new BlobServiceClient(connectionString);

            // Pobierz nazwę kontenera z konfiguracji
            _containerName = _configuration.GetSection("BlobStorage")["ContainerName"];
        }
        public async Task<(bool IsSuccess, string Message)> AddToFavAnn(string userId, int id)
        {
            var announcement = await _dbContext.Announcements
                .Where(a => a.AnId == id)
                .FirstOrDefaultAsync();

            if (announcement == null)
            {
                return (false, "Announcement does not exist.");
            }

            var existingFavorite = await _dbContext.FavoriteAnnouncements
                .Where(fa => fa.UserId == int.Parse(userId) && fa.AnnouncementAnId == id)
                .FirstOrDefaultAsync();

            if (existingFavorite != null)
            {
                return (false, "This announcement is already in your favorites.");
            }

            var newFavorite = new FavoriteAnnouncements
            {
                UserId = int.Parse(userId),
                //FavoriteAnnouncementId = id,
                AnnouncementAnId = id

            };

            _dbContext.FavoriteAnnouncements.Add(newFavorite);
            await _dbContext.SaveChangesAsync();

            return (true, "Announcement added to favorites successfully.");
        }
        public async Task<Comment> CreateCom(CommentCreateRequest commentCreateRequest, string usr)
        {
            var comment = new Comment()
            {
                UserId = int.Parse(usr),
                AnId = commentCreateRequest.AnId,
                CommentText = commentCreateRequest.CommentText
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();


            return comment;
        }
        public async Task<bool> DeleteAnnFromFavorites(int AnnId, int userId)
        {

            var fvann = await _dbContext.FavoriteAnnouncements
                                        .FirstOrDefaultAsync(f => f.AnnouncementAnId == AnnId && f.UserId == userId);

            if (fvann == null)
            {
                throw new ArgumentException("The specified announcement was not found in the user's favorites.");
            }

            _dbContext.FavoriteAnnouncements.Remove(fvann);
            await _dbContext.SaveChangesAsync();

            return true;

        }
        public async Task<bool> DeleteCom(int commentId, int userId)
        {
            var comment = await _dbContext.Comments.FindAsync(commentId);

            if (comment == null || comment.UserId != userId)
            {
                return false;
            }

            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        public async Task<List<AnnouncementDto>> GetFvAnnsByUsrId(int id)
        {
            var announcementIds = await _dbContext.FavoriteAnnouncements
    .Where(f => f.UserId == id)
    .Select(f => f.AnnouncementAnId) // Pobranie tylko potrzebnych ID ogłoszeń
    .ToListAsync();

            var favoriteAnnouncements = await _dbContext.Announcements
                .Where(a => announcementIds.Contains(a.AnId)) // Filtrowanie po pobranych ID
                .Include(a => a.Images) // Załadowanie powiązanych obrazów
                .AsNoTracking()
                .ToListAsync();

            if (favoriteAnnouncements == null) return null;

            var comDto = _mapper.Map<List<AnnouncementDto>>(favoriteAnnouncements);

            return comDto;
        }
        public async Task<Announcement> CreateAnn(AnnouncementCreateRequest request, string usr)
        {
            var slug = await GenerateUniqueSlugAsync(request.Brand, request.Model);

            string title = string.IsNullOrWhiteSpace(request.summary) ? " " : request.summary;

            var user = _dbContext.Users.FirstOrDefault(f => f.UserId == int.Parse(usr));
            if (user == null) throw new Exception("User not found.");

            var announcement = new Announcement
            {
                Brand = request.Brand,
                Model = request.Model,
                Slug = slug,
                isActive = true,
                UserId = int.Parse(usr),
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Category = request.Category,
                Description = request.Description,
                City = user.City,
                Years = request.Years,
                Summary = title,
                lat = user.lat,
                lng = user.lng,
                Comments = new List<Comment>(),
                Images = new List<AnnouncementImages>()
            };

            _dbContext.Announcements.Add(announcement);
            await _dbContext.SaveChangesAsync();

            int imageCount = 0;

            foreach (var file in request.Images)
            {
                if (file.Length > 0)
                {
                    if (!IsValidImage(file))
                    {
                        throw new Exception("Invalid file type. Only image files are allowed.");
                    }
                    imageCount++;

                    var fileName = $"{imageCount}-{announcement.Slug}{Path.GetExtension(file.FileName)}";

                    // Tworzymy kontener (jeśli nie istnieje)
                    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                    await blobContainerClient.CreateIfNotExistsAsync();

                    // Utwórz obiekt BlobClient
                    var blobClient = blobContainerClient.GetBlobClient(fileName);

                    using (var stream = file.OpenReadStream())
                    {
                        // Prześlij plik do Azurite
                        await blobClient.UploadAsync(stream);
                    }

                    var imageUrl = $"http://localhost:10000/{_containerName}/{fileName}"; // Azurite URL

                    var image = new AnnouncementImages
                    {
                        AnId = announcement.AnId,
                        ImageUrl = imageUrl
                    };

                    _dbContext.AnnouncementImages.Add(image);
                    announcement.Images.Add(image);
                }
            }

            await _dbContext.SaveChangesAsync();
            return announcement;
        }

        //public async Task<bool> DeleteAnnouncementAsync(int announcementId, int userId)
        //{

        //    var announcement = await _dbContext.Announcements
        //        .Include(a => a.Comments)
        //        .Include(a => a.Images)
        //        .Include(a => a.FavoriteAnnouncements)
        //        .FirstOrDefaultAsync(a => a.AnId == announcementId && a.UserId == userId);

        //    if (announcement == null)
        //    {
        //        return false;
        //    }

        //    _dbContext.Comments.RemoveRange(announcement.Comments);
        //    _dbContext.AnnouncementImages.RemoveRange(announcement.Images);
        //    _dbContext.FavoriteAnnouncements.RemoveRange(announcement.FavoriteAnnouncements);


        //    var blobContainerClient = _blobServiceClient.GetBlobContainerClient("automovieimages");
        //    foreach (var image in announcement.Images)
        //    {
        //        var blobUri = new Uri(image.ImageUrl);
        //        var blobClient = new BlobClient(blobUri);
        //        var blobName = blobClient.Name;

        //        var containerBlobClient = blobContainerClient.GetBlobClient(blobName);
        //        await containerBlobClient.DeleteIfExistsAsync();
        //    }


        //    _dbContext.Announcements.Remove(announcement);

        //    await _dbContext.SaveChangesAsync();

        //    return true;
        //}

        private static readonly List<string> AllowedMimeTypes = new()
{
    "image/jpeg",
    "image/png",
    "image/gif",
    "image/webp"
};

        private bool IsValidImage(IFormFile file)
        {
            return AllowedMimeTypes.Contains(file.ContentType.ToLower());
        }

        public async Task<bool> DeleteAnnouncementAsync(int announcementId, int userId)
        {
            // Znajdź ogłoszenie w bazie danych
            var announcement = await _dbContext.Announcements
                .Include(a => a.Comments)
                .Include(a => a.Images)
                .Include(a => a.FavoriteAnnouncements)
                .FirstOrDefaultAsync(a => a.AnId == announcementId && a.UserId == userId);

            if (announcement == null)
            {
                return false;
            }

            // Usuń powiązane dane z bazy danych
            _dbContext.Comments.RemoveRange(announcement.Comments);
            _dbContext.AnnouncementImages.RemoveRange(announcement.Images);
            _dbContext.FavoriteAnnouncements.RemoveRange(announcement.FavoriteAnnouncements);

            // Tworzymy klienta kontenera
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            // Usuwanie obrazów z Azurite
            foreach (var image in announcement.Images)
            {
                // Wyciągnij nazwę obiektu (blob) z URL obrazu
                var blobName = new Uri(image.ImageUrl).Segments.LastOrDefault();
                if (!string.IsNullOrEmpty(blobName))
                {
                    try
                    {
                        var blobClient = blobContainerClient.GetBlobClient(blobName);
                        await blobClient.DeleteIfExistsAsync();  // Usuwamy blob z Azurite
                    }
                    catch (Exception ex)
                    {
                        // Można dodać logowanie w przypadku błędu
                        Console.WriteLine($"Error deleting blob {blobName}: {ex.Message}");
                    }
                }
            }

            // Usuń ogłoszenie z bazy danych
            _dbContext.Announcements.Remove(announcement);

            // Zapisz zmiany w bazie danych
            await _dbContext.SaveChangesAsync();

            return true;
        }




        public async Task<string> GenerateUniqueSlugAsync(string brand, string model, int suffixLength = 8)
        {
            string slug;

            do
            {
                var numericSuffix = GenerateNumericSuffix(suffixLength);
                slug = $"{brand}-{model}-{numericSuffix}".ToLower();
            } while (await _dbContext.Announcements.AnyAsync(a => a.Slug == slug));

            return slug;
        }

        public string GenerateNumericSuffix(int length = 8)
        {
            var random = new Random();
            var suffix = new char[length];

            for (int i = 0; i < length; i++)
            {
                suffix[i] = (char)('0' + random.Next(0, 10));
            }

            return new string(suffix);
        }
    }
}
