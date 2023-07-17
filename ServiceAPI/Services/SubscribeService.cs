using ServiceAPI.Models;
using ServiceAPI.Services.Interfaces;
using ServiceAPI.UnitOfWork.Interfaces;

namespace ServiceAPI.Services
{
    public class SubscribeService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubscribeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            await _unitOfWork.Save();
        }
    }
}
