using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ApiClothes.Entities
{
    public class PlatformDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        DbSet<Announcement> Announcements { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<AnnouncementImages> AnnouncementImages { get; set; }
        DbSet<FavoriteAnnouncements> FavoriteAnnouncements { get;set; }

        public PlatformDbContext(DbContextOptions<PlatformDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(a => a.User)
                    .WithMany(u => u.Announcements)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(a => a.Comments)
                    .WithOne(c => c.Announcement)
                    .HasForeignKey(c => c.AnId);

                entity.HasMany(a => a.Images)
                    .WithOne(ai => ai.Announcement)
                    .HasForeignKey(ai => ai.AnId);
            });

            modelBuilder.Entity<Comment>()
         .HasOne(c => c.User)
         .WithMany(u => u.Comments)
         .HasForeignKey(c => c.UserId)
         .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Announcement)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AnId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AnnouncementImages>()
                .HasOne(ai => ai.Announcement)
                .WithMany(a => a.Images)
                .HasForeignKey(ai => ai.AnId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
            }
        }

    }
}
