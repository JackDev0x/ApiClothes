using ApiClothes.Entities;
using ApiClothes.RequestsModels;

namespace ApiClothes.Services.Interfaces
{
    public interface IAccountManager
    {
        public Task<Announcement> CreateAnn(AnnouncementCreateRequest request, string usr);
        Task<bool> DeleteAnnouncementAsync(int announcementId, int userId);


    }
}
    