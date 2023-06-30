using MilienAPI.Models;
using MilienAPI.Services.Interfaces;
using MilienAPI.UnitOfWork.Interfaces;

namespace MilienAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
             _unitOfWork = unitOfWork;
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            var customer = await _unitOfWork.Customers.GetById(id);

            return customer;
        }
    }
}
