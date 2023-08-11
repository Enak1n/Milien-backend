using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Millien.Domain.UnitOfWork.Interfaces;
using ServiceAPI.Services.Interfaces;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ServiceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IUnitOfWork _unitOfWork;

        public ChatController(IMessageService messageService, IUnitOfWork unitOfWork)
        {
            _messageService = messageService;
            _unitOfWork = unitOfWork;
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> FindCorrespondence(string query)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var res = await _messageService.FindCorrespondence(query, userId);

                return Ok(res);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CountOfUnreadingMessages()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var res = await _messageService.CountOfUnreadingMessages(userId);

                return Ok(res);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
