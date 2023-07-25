using Microsoft.Extensions.Caching.Memory;
using MilienAPI.DataBase;
using MilienAPI.Models;
using MilienAPI.UnitOfWork.Interfaces;

namespace MilienAPI.UnitOfWork
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(Context context, IMemoryCache memoryCache) : base(context, memoryCache)
        {

        }
    }

}
