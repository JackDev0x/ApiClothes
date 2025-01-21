using ApiClothes.Services.Interfaces;
using ApiClothes.DtoModels;
using Microsoft.EntityFrameworkCore;
using ApiClothes.Entities;
using AutoMapper;

namespace ApiClothes.Services.Services
{
    public class ProductCatalog : IProductCatalog
    {
        private readonly PlatformDbContext _dbContext;
        private readonly IMapper _mapper;
        //private readonly BlobServiceClient _blobServiceClient;

        public ProductCatalog(PlatformDbContext dbContext, IMapper mapper/*, BlobServiceClient blobServiceClient*/)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            //_blobServiceClient = blobServiceClient;
        }
        public Task<List<AnnouncementDto>> GetAnnsByUsrId(int id)
        {
            throw new NotImplementedException();
        }

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

        public Task<AnnouncementDto> GetBySlug(string slug)
        {
            throw new NotImplementedException();
        }

        public Task<List<CommentDto>> GetCommentsByAnnId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUsrById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
