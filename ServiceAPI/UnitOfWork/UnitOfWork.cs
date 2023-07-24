﻿using Microsoft.Extensions.Caching.Memory;
using MilienAPI.UnitOfWork.Interfaces;
using ServiceAPI.Data;
using ServiceAPI.UnitOfWork.Interfaces;

namespace ServiceAPI.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;

        public UnitOfWork(Context context, IMemoryCache memoryCache)
        {
             _context = context;
            PaidAds = new PaidAdRepository(context, memoryCache);
            Subscriptions = new SubscriptionRepository(context, memoryCache);
            Customers = new CustomerRepository(context, memoryCache);
        }

        public IPaidAdRepository PaidAds { get; private set; }
        public ISubscriptionRepository Subscriptions { get; private set; }
        public ICustomerRepository Customers { get; private set; }

        public UnitOfWork(IPaidAdRepository paidAds, ISubscriptionRepository subscriptions, ICustomerRepository customerRepository)
        {
            PaidAds = paidAds;
            Subscriptions = subscriptions;
            Customers = customerRepository;
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
