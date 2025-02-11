using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private readonly IAvatarService _avatarService;

        public AvatarController(IAvatarService avatarService)
        {
            _avatarService = avatarService;
        }

        [HttpPost("upload-avatar")]
        [Authorize]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            try
            {
                var userInfo = await _avatarService.UploadAvatarAsync(file, User);
                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("get-avatar-url")]
        [Authorize]
        public async Task<IActionResult> GetAvatar()
        {
            var avatarUrl = await _avatarService.GetAvatarUrlAsync(User);
            if (string.IsNullOrEmpty(avatarUrl))
                return NotFound("Avatar not found.");

            return Ok(new { avatarUrl });
        }
    }

}
