using ApiClothes.Entities;
using ApiClothes.DtoModels;
using ApiClothes.RequestsModels;

namespace ApiClothes.Services.Interfaces
{
    public interface IProductCatalog
    {
        //Task<(bool IsSuccess, string Message)> AddToFavAnn(string userId, int id);
        Task<AnnouncementDto> GetById(int id);
        Task<AnnouncementDto> GetBySlug(string slug);
        Task<UserDto> GetUsrById(int id);
        public Task<List<UserDto>> GetUsers();
        Task<List<CommentDto>> GetCommentsByAnnId(int id);
        public Task<List<AnnouncementDto>> GetAnnsByUsrId(int id);
        Task<PagedResult<AnnouncementDto>> GetAnnouncementsAsync(PaginationParameters paginationParameters);

    }
}
