﻿using Microsoft.Extensions.Caching.Memory;
using ServiceAPI.Data;
using ServiceAPI.Models;
using ServiceAPI.UnitOfWork.Interfaces;

namespace ServiceAPI.UnitOfWork
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(Context context, IMemoryCache memoryCache) : base(context, memoryCache) { }

    }
}
