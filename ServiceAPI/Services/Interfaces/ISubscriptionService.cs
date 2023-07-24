﻿using ServiceAPI.Models;

namespace ServiceAPI.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task Subscribe(int followerId, int followingId);
        Task Unsubscribe(int followerId, int followingId);
        Task<bool> IsSubscribe(int userId, int followingId);
        Task<List<Customer>> GetMySubscriptions(int userId);
    }
}