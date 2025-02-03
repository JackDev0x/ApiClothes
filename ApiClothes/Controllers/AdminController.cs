using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Storage;
using ApiClothes.RequestsModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ApiClothes.Services.Interfaces;
using ApiClothes.Entities;
using Microsoft.EntityFrameworkCore;
using ApiClothes.Services.Services;

namespace ApiClothes.Controllers
{
    [Route("api/Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IProductCatalog _productCatalog;
        private readonly PlatformDbContext _dbContext;
        public AdminController(IProductCatalog productCatalog, PlatformDbContext dbContext)
        {
            _productCatalog = productCatalog;
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getUsersAdmin")]
        public async Task<IActionResult> GetUsersAdmin()
        {
            var users = await _productCatalog.GetUsers();

            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUser( int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteAnnouncement/{AnnId}")]
        public async Task<IActionResult> DeleteAnnouncement(int AnnId)
        {
            var ann = await _dbContext.Announcements
                .Include(a => a.Comments)
                .Include(a => a.Images)
                .Include(a => a.FavoriteAnnouncements)
                .FirstOrDefaultAsync(a => a.AnId == AnnId);

            if (ann == null)
            {
                return NotFound(new { message = "Announcement not found" });
            }

            // Jeśli nie masz Cascade Delete w bazie, usuń powiązane dane ręcznie:
            if (ann.Comments != null && ann.Comments.Any())
                _dbContext.Comments.RemoveRange(ann.Comments);

            if (ann.Images != null && ann.Images.Any())
                _dbContext.AnnouncementImages.RemoveRange(ann.Images);

            if (ann.FavoriteAnnouncements != null && ann.FavoriteAnnouncements.Any())
                _dbContext.FavoriteAnnouncements.RemoveRange(ann.FavoriteAnnouncements);

            _dbContext.Announcements.Remove(ann);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Announcement deleted successfully" });
        }

    }
}