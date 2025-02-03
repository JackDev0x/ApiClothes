using ApiClothes.Entities;
using ApiClothes.RequestsModels;
using ApiClothes.DtoModels;

namespace ApiClothes.Services.Interfaces
{
    public interface IAccountManager
    {
        public Task<Announcement> CreateAnn(AnnouncementCreateRequest request, string usr);
        Task<bool> DeleteAnnouncementAsync(int announcementId, int userId);

        Task<List<AnnouncementDto>> GetFvAnnsByUsrId(int id);
        Task<Comment> CreateCom(CommentCreateRequest request, string usr);
        Task<bool> DeleteCom(int commentId, int userId);
        Task<bool> DeleteAnnFromFavorites(int annId, int userId);
        Task<(bool IsSuccess, string Message)> AddToFavAnn(string userId, int id);


    }
}
    