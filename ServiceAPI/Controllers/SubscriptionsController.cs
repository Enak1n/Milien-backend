using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAPI.Services.Interfaces;
using System.Security.Claims;

namespace ServiceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Subscribe(int followingId)
        {
            var authorizedUser = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                await _subscriptionService.Subscribe(authorizedUser, followingId);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Unubscribe(int followingId)
        {
            var authorizedUser = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                await _subscriptionService.Unsubscribe(authorizedUser, followingId);

                return Ok();
            }
            catch { return BadRequest(); }
        }
    }
}
