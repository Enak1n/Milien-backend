using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAPI.Services.Interfaces;
using ServiceAPI.UnitOfWork.Interfaces;
using System.Security.Claims;

namespace ServiceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionsController(ISubscriptionService subscriptionService, IUnitOfWork unitOfWork)
        {
            _subscriptionService = subscriptionService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Subscribe([FromBody]int followingId)
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

                await _unitOfWork.Save();

                return Ok();
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<bool> IsSubscribe(int followingId)
        {
            var authorizedUser = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                return await _subscriptionService.IsSubscribe(authorizedUser, followingId);
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCountOfSubscribers(int userId)
        {
            var count = await _unitOfWork.Subscriptions.FindRange(s => s.FollowingId == userId);

            return Ok(count.Count);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMySubscriptions()
        {
            var authorizedUser = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var mySubscriptions = await _subscriptionService.GetMySubscriptions(authorizedUser);
                return Ok(mySubscriptions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
