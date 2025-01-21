using ApiClothes.Entities;

namespace ApiClothes.DtoModels
{
    public class AnnouncementImagesDto
    {
        public int ImageId { get; set; }
        public int AnId { get; set; }
        public string ImageUrl { get; set; }
        public virtual AnnouncementDto Announcement { get; set; }
    }
}
