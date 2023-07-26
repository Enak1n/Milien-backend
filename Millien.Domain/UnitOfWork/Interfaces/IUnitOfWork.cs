using Millien.Domain.Entities;

namespace Millien.Domain.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAddRepository Ads { get; }
        IFavoriteRepository Favorites { get; }
        IPaidAdRepository PaidAds { get; }
        ISubscriptionRepository Subscriptions { get; }
        ICustomerRepository Customers { get; }
        INotificationRepository Notifications { get; }

        Task Save();
    }
}
