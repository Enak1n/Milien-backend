﻿namespace MilienAPI.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAddRepository Ads { get; }
        IFavoriteRepository Favorites { get; }
        ICustomerRepository Customers { get; }
        IPaidAdRepository PaidAd { get; }
        ISubscriptionRepository Subscriptions { get; }
        INotificationRepository Notifications { get; }

        Task Save();
    }
}
