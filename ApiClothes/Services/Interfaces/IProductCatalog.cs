using ApiClothes.Entities;
using ApiClothes.DtoModels;

namespace ApiClothes.Services.Interfaces
{
    public interface IProductCatalog
    {
        //Task<(bool IsSuccess, string Message)> AddToFavAnn(string userId, int id);
        Task<AnnouncementDto> GetById(int id);
        Task<AnnouncementDto> GetBySlug(string slug);
        Task<UserDto> GetUsrById(int id);
        Task<List<CommentDto>> GetCommentsByAnnId(int id);
        Task<List<AnnouncementDto>> GetAnnsByUsrId(int id);
        //Task<List<FavoriteAnnouncementsDto>> GetFvAnnsByUsrId(int id);
        //Task<Announcement> CreateAnn(AnnouncementCreateRequest request, string usr);
        //Task<Comment> CreateCom(CommentCreateRequest request, string usr);
        //Task<bool> DeleteCom(int commentId, int userId);
        //Task<bool> DeleteAnnouncementAsync(int announcementId, int userId);
        //Task<bool> DeleteAnnFromFavorites(int annId, int userId);
    }
}
