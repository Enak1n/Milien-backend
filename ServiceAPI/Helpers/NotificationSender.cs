using ServiceAPI.Data;
using ServiceAPI.Models;
using ServiceAPI.UnitOfWork.Interfaces;

namespace ServiceAPI.Helpers
{
    public static class NotificationSender
    {
        public static async Task SendNotification(string message, int userId, IUnitOfWork unitOfWork)
        {
            var users = await unitOfWork.Subscriptions.FindRange(s => s.FollowingId == userId);

            foreach (var user in users) 
            {
                await unitOfWork.Notifications.Add(new Notification(message, user.FollowingId, user.FollowerId));
            }

            await unitOfWork.Save();
        }

        public static async Task SendNotificationForIndividualUser(string message, int userId, int subscriberId, IUnitOfWork unitOfWork)
        {
            var users = await unitOfWork.Customers.Find(u => u.Id == userId);
            await unitOfWork.Notifications.Add(new Notification(message, subscriberId, userId));
            await unitOfWork.Save();
        }
    }
}
