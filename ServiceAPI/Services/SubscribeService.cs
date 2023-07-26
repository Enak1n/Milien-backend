using Millien.Domain.Entities;
using ServiceAPI.Services.Interfaces;
using Millien.Domain.UnitOfWork.Interfaces;

namespace ServiceAPI.Services
{
    public class SubscribeService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubscribeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Customer>> GetMySubscriptions(int ownerId)
        {
            var subscribes = await _unitOfWork.Subscriptions.FindRange(s => s.FollowerId == ownerId);
            var subscribesId = subscribes.Select(a => a.FollowingId).ToList();

            var customersWithFollowingIds = await _unitOfWork.Customers
                                                  .FindRange(cust => subscribesId.Contains(cust.Id));

            return customersWithFollowingIds;
        }

        public async Task<List<Notification>> GetNotifications(int ownerId)
        {
            var res = await _unitOfWork.Notifications.FindRange(s => s.OwnerId == ownerId);

            var notifications = res.OrderByDescending(s => s.DateOfCreation).ToList(); 

            return notifications;
        }

        public async Task<bool> IsSubscribe(int userId, int followingId)
        {
            var subscription = await _unitOfWork.Subscriptions.Find(s => s.FollowerId == userId && s.FollowingId == followingId);

            return subscription != null;
        }

        public async Task RemoveFromNotification(int userId)
        {
            var notifications = await _unitOfWork.Notifications.FindRange(n => n.OwnerId == userId);

            await _unitOfWork.Notifications.RemoveRange(notifications);
        }

        public async Task Subscribe(int followerId, int followingId)
        {
            var subscription = new Subscription(followerId, followingId);

            await _unitOfWork.Subscriptions.Add(subscription);
            await _unitOfWork.Save();
        }

        public async Task Unsubscribe(int followerId, int followingId)
        {
            var currentSubscription = await _unitOfWork.Subscriptions.Find(a => a.FollowerId == followerId && a.FollowingId == followingId);

            await _unitOfWork.Subscriptions.Remove(currentSubscription);
        }
    }
}
