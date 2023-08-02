using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAPI.Services.Interfaces;
using System.Security.Claims;

namespace ServiceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public ChatController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllCorresponences()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var res = await _messageService.GetAllCorresponences(userId);

                return Ok(res);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCorrespondence(int recipientId)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var res = await _messageService.GetCorrespondence(userId, recipientId);

                return Ok(res);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
