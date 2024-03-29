﻿using Millien.Domain.Entities;
using MilienAPI.Models.Requests;

namespace MilienAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<Customer> GetCustomerById(int id);
        Task<(Customer, List<Ad>)> GetOwnAds(int userId);
        Task EditProfile(int userId, AccountRequest accountResponse);
    }
}
