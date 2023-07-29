using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAPI.Helpers;
using Millien.Domain.Entities;
using ServiceAPI.Models.Responses;
using ServiceAPI.Services.Interfaces;
using Millien.Domain.UnitOfWork.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace ServiceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<UserStatusHub> _hubContext;

        public SubscriptionsController(ISubscriptionService subscriptionService, IUnitOfWork unitOfWork, IMapper mapper,
            IHubContext<UserStatusHub> hubContext)
        {
            _subscriptionService = subscriptionService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Subscribe([FromBody] int followingId)
        {
            var authorizedUser = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                await _subscriptionService.Subscribe(authorizedUser, followingId);
                var user = await _unitOfWork.Customers.Find(u => u.Id == authorizedUser);
                await NotificationSender.SendNotificationForIndividualUser($"Пользователь подписался на ваши обновления!", followingId, authorizedUser, _unitOfWork);
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
            catch (Exception ex)
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetNotifications()
        {
            var ownerId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var res = await _subscriptionService.GetNotifications(ownerId);
            List<NotificationResponse> notificationResponses = new(); 

            foreach (var notification in res)
            {
                var user = await _unitOfWork.Customers.Find(u => u.Id == notification.CustomerId);
                var notifications = _mapper.Map<Notification, NotificationResponse>(notification);
                notifications.Customer = user;
                notificationResponses.Add(notifications);
            }

            return Ok(notificationResponses);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveNotifications()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _subscriptionService.RemoveFromNotification(userId);

            await _unitOfWork.Save();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Test(int id)
        {
            return Ok();
        }
    }
}
