using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ApiClothes.Entities
{
    public class Announcement
    {
        [Key]
        public int AnId { get; set; }
        public string Slug { get; set; }
        public int UserId { get; set; }
        public int Years { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string Summary { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
        public virtual User User { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<AnnouncementImages> Images { get; set; }
        public virtual ICollection<FavoriteAnnouncements> FavoriteAnnouncements { get; set; }
    }
}