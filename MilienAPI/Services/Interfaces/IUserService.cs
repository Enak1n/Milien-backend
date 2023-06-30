using MilienAPI.Models;

namespace MilienAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<Customer> GetCustomerById(int id);
    }
}
