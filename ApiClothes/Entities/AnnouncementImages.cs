using System.ComponentModel.DataAnnotations;

namespace ApiClothes.Entities
{
    public class AnnouncementImages
    {
        [Key]
        public int ImageId { get; set; }
        public int AnId { get; set; }
        public string ImageUrl { get; set; }
        public virtual Announcement Announcement { get; set; }
    }
}
