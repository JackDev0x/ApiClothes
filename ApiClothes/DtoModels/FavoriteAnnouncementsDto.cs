namespace ApiClothes.DtoModels
{
    public class FavoriteAnnouncementsDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AnnouncementId { get; set; }

        public virtual UserDto User { get; set; }
        public virtual AnnouncementDto Announcement { get; set; }
    }
}
