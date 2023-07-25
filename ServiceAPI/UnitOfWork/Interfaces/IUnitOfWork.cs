using ServiceAPI.UnitOfWork.Interfaces;

namespace ServiceAPI.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPaidAdRepository PaidAds { get; }
        ISubscriptionRepository Subscriptions { get; }
        ICustomerRepository Customers { get; }
        INotificationRepository Notifications { get; }

        Task Save();
    }
}
