using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ApiClothes.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Proszę podać imię")]
        public string Name { get; set; }

        public string? Surname { get; set; }
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Proszę podać adres e-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public double lat { get; set; }
        public double lng { get; set; }
        public string? Voivodeship { get; set; }
        public string? City { get; set; }
        public virtual List<Announcement> Announcements { get; set; }
        public virtual List<FavoriteAnnouncements> FavoriteAnnouncements { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}
