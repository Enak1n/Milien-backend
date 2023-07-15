using MilienAPI.Models;
using MilienAPI.Models.Requests;
using MilienAPI.Models.Responses;

namespace MilienAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<Customer> GetCustomerById(int id);
        Task<(Customer, List<Ad>)> GetOwnAds(int userId);
        Task EditProfile(int userId, AccountRequest accountResponse);
    }
}
