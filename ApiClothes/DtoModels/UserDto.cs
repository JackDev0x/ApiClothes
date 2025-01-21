using ApiClothes.Entities;
using System.ComponentModel.DataAnnotations;

namespace ApiClothes.DtoModels
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }

        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string? Voivodeship { get; set; }
        public string? City { get; set; }
        public virtual List<Announcement> Announcements { get; set; }
        public virtual List<FavoriteAnnouncements> FavoriteAnnouncements { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}
