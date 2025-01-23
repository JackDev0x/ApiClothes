using ApiClothes.Services.Interfaces;
using ApiClothes.DtoModels;
using Microsoft.EntityFrameworkCore;
using ApiClothes.Entities;
using ApiClothes.RequestsModels;
using AutoMapper;

namespace ApiClothes.Services.Services
{
    public class ProductCatalog : IProductCatalog
    {
        private readonly PlatformDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductCatalog> _logger;

        //private readonly BlobServiceClient _blobServiceClient;

        public ProductCatalog(PlatformDbContext dbContext, IMapper mapper, ILogger<ProductCatalog> logger/*, BlobServiceClient blobServiceClient*/)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            //_blobServiceClient = blobServiceClient;
        }
        //public Task<List<AnnouncementDto>> GetAnnsByUsrId(int id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<AnnouncementDto> GetById(int id)
        {
            var announcement = await _dbContext.Announcements
                .Include(a => a.FavoriteAnnouncements)
        .Include(a => a.User)
        .Include(a => a.Comments)
            .ThenInclude(c => c.User)
        .Include(a => a.Images)
        .AsQueryable().AsNoTracking()
        .FirstOrDefaultAsync(a => a.AnId == id);

            if (announcement == null) return null;

            var announcementDto = _mapper.Map<AnnouncementDto>(announcement);
            announcementDto.LikedBy = _dbContext.FavoriteAnnouncements.Where(s => s.AnnouncementAnId == id).Select(fa => fa.UserId).ToList();

            return announcementDto;
        }

        public async Task<AnnouncementDto> GetBySlug(string slug)
        {
            var announcement = await _dbContext.Announcements
                .Include(a => a.FavoriteAnnouncements)
        .Include(a => a.User)
        .Include(a => a.Comments)
            .ThenInclude(c => c.User)
        .Include(a => a.Images)
        .AsQueryable().AsNoTracking()
        .FirstOrDefaultAsync(a => a.Slug == slug);

            if (announcement == null) return null;

            var announcementDto = _mapper.Map<AnnouncementDto>(announcement);
            var id = announcement.AnId;
            announcementDto.LikedBy = _dbContext.FavoriteAnnouncements.Where(s => s.AnnouncementAnId == id).Select(fa => fa.UserId).ToList();

            return announcementDto;
        }

        public async Task<PagedResult<AnnouncementDto>> GetAnnouncementsAsync(PaginationParameters paginationParameters)
        {
            _logger.LogInformation("Fetching announcements with PageNumber: {PageNumber}, PageSize: {PageSize}",
                                   paginationParameters.PageNumber, paginationParameters.PageSize);

            var query = _dbContext.Announcements.Include(a => a.Images).AsQueryable();

            var totalCount = await query.CountAsync();
            _logger.LogInformation("Total announcements count: {TotalCount}", totalCount);

            var paginatedData = await query
                .OrderByDescending(a => a.DatePosted)
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync();

            _logger.LogInformation("Fetched {Count} announcements for PageNumber: {PageNumber}",
                                   paginatedData.Count, paginationParameters.PageNumber);

            var result = _mapper.Map<List<AnnouncementDto>>(paginatedData);

            return new PagedResult<AnnouncementDto>
            {
                TotalCount = totalCount,
                PageSize = paginationParameters.PageSize,
                CurrentPage = paginationParameters.PageNumber,
                TotalPages = (int)Math.Ceiling((double)totalCount / paginationParameters.PageSize),
                Items = result
            };
        }

        public Task<UserDto> GetUsrById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CommentDto>> GetCommentsByAnnId(int id)
        {
            var com = await _dbContext.Comments.Where(f => f.AnId == id).Include(a => a.User).AsQueryable().AsNoTracking()
                .ToListAsync();

            if (com == null) return null;

            var comDto = _mapper.Map<List<CommentDto>>(com);

            return comDto;
        }
        public async Task<List<AnnPreview>> GetAnnsByUsrId(int id)
        {
            var anns = await _dbContext.Announcements
                .Include(a => a.FavoriteAnnouncements)
                .Include(a => a.User)
                .Include(a => a.Images)
                .Where(f => f.UserId == id).AsQueryable().AsNoTracking()
                .ToListAsync();

            if (anns == null) return null;

            var projectedResults = anns.Select(a => new AnnPreview
            {
                Id = a.AnId,
                Slug = a.Slug,
                Brand = a.Brand,
                Model = a.Model,
                Description = a.Description,
                summary = a.Summary,
                User = new UserDto
                {
                    UserId = a.User.UserId,
                    Name = a.User.Name,
                    Surname = a.User.Surname,
                    Phone = a.User.Phone,
                    Email = a.User.Email,
                    lat = a.User.lat,
                    lng = a.User.lng,
                    Voivodeship = a.User.Voivodeship,
                    City = a.User.City,
                },
                Price = a.Price,
                ProductionYear = a.Years,
                LikedBy = _dbContext.FavoriteAnnouncements
                            .Where(fa => fa.AnnouncementAnId == a.AnId)
                            .Select(fa => fa.UserId)
                            .ToList(),
                Images = a.Images.Select(i => new AnnouncementImagesDto
                {
                    AnId = i.AnId,
                    ImageUrl = i.ImageUrl
                }).ToList()
            }).ToList();

            return projectedResults;
        }

        public async Task<List<FavoriteAnnouncementsDto>> GetFvAnnsByUsrId(int id)
        {
            var com = await _dbContext.FavoriteAnnouncements.Where(f => f.UserId == id).AsQueryable().AsNoTracking()
                .ToListAsync();

            if (com == null) return null;

            var comDto = _mapper.Map<List<FavoriteAnnouncementsDto>>(com);

            return comDto;
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
        

        Task<List<AnnouncementDto>> IProductCatalog.GetAnnsByUsrId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
