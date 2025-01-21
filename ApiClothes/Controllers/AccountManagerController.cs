using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Storage;
using ApiClothes.RequestsModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ApiClothes.Services.Interfaces;

namespace ApiClothes.Controllers
{
    [Route("api/AccountManager")]
    public class AccountManagerController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        public AccountManagerController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        [Authorize]
        [HttpPost("CreateAnnouncement")]
        public async Task<IActionResult> CreateAnnouncement([FromForm] AnnouncementCreateRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



                var announcement = await _accountManager.CreateAnn(request, userId);

                return Ok(announcement);

            }
            catch (HttpRequestException httpEx)
            {
                return StatusCode(500, httpEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("DeleteAnnouncement/{AnnId}")]
        public async Task<IActionResult> DeleteAnnouncement(int AnnId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _accountManager.DeleteAnnouncementAsync(AnnId, userId);

            if (!result)
            {
                return NotFound(new { Message = "Announcement not found or user not authorized." });
            }

            return Ok(new { Message = "Announcement deleted successfully." });
        }


    }
}
