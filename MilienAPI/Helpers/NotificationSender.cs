using MilienAPI.DataBase;
using MilienAPI.Models;
using MilienAPI.UnitOfWork.Interfaces;

namespace MilienAPI.Helpers
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
    }
}
