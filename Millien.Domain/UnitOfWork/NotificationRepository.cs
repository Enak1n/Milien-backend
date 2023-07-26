using Microsoft.Extensions.Caching.Memory;
using Millien.Domain.DataBase;
using Millien.Domain.Entities;
using Millien.Domain.UnitOfWork.Interfaces;

namespace Millien.Domain.UnitOfWork
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(Context context, IMemoryCache memoryCache) : base(context, memoryCache)
        {

        }
    }

}
