using System.ComponentModel.DataAnnotations;

namespace ApiClothes.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int AnId { get; set; }

        [Required(ErrorMessage = "Tresc komentarza nie moze byc pusta")]
        public string CommentText { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;

        public virtual Announcement Announcement { get; set; }
        public virtual User User { get; set; }
    }
}
