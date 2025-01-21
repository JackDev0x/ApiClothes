using ApiClothes.Entities;
using System.ComponentModel.DataAnnotations;

namespace ApiClothes.DtoModels
{
    public class AnnouncementDto
    {
        
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
            public DateTime DatePosted { get; set; } = DateTime.UtcNow;
            public virtual UserDto User { get; set; }
            public virtual List<CommentDto> Comments { get; set; }
            public virtual List<AnnouncementImagesDto> Images { get; set; }
            public List<int> LikedBy { get; set; }


    }
}
