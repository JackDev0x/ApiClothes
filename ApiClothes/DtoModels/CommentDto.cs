using ApiClothes.Entities;
using System.ComponentModel.DataAnnotations;

namespace ApiClothes.DtoModels
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int AnId { get; set; }

        public string CommentText { get; set; }

        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
        public virtual AnnouncementDto Announcement { get; set; }
        public virtual UserDto User { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
