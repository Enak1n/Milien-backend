using Microsoft.Extensions.Caching.Memory;
using ServiceAPI.Data;
using ServiceAPI.Models;
using ServiceAPI.UnitOfWork.Interfaces;

namespace ServiceAPI.UnitOfWork
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(Context context, IMemoryCache memoryCache) : base(context, memoryCache)
        {

        }
    }

}
