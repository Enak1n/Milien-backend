using Microsoft.Extensions.Caching.Memory;
using Millien.Domain.DataBase;
using Millien.Domain.UnitOfWork.Interfaces;

namespace Millien.Domain.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;

        public UnitOfWork(Context context, IMemoryCache memoryCache)
        {
             _context = context;
            Ads = new AdRepository(context, memoryCache);
            Favorites = new FavoriteRepository(context, memoryCache);
            Customers = new CustomerRepository(context, memoryCache);
            PaidAds = new PaidAdRepository(context, memoryCache);
            Subscriptions = new SubscriptionRepository(context, memoryCache);
            Notifications = new NotificationRepository(context, memoryCache);
            Messages = new MessageRepository(context, memoryCache);
        }
        public IAddRepository Ads { get; private set; }
        public IFavoriteRepository Favorites { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IPaidAdRepository PaidAds { get; private set; }
        public ISubscriptionRepository Subscriptions { get; private set; }
        public INotificationRepository Notifications { get; private set; }
        public IMessageRepository Messages { get; private set; }

        public UnitOfWork(IAddRepository addRepository, IFavoriteRepository favorites, ICustomerRepository customers, IPaidAdRepository paidAds,
            ISubscriptionRepository subscriptions, INotificationRepository notification, IMessageRepository messageRepository)
        {
            Ads = addRepository;
            Favorites = favorites;
            Customers = customers;
            PaidAds = paidAds;
            Subscriptions = subscriptions;
            Notifications = notification;
            Messages = messageRepository;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
           _context.Dispose();
        }
    }
}
