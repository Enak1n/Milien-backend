using MilienAPI.Exceptions;
using MilienAPI.Models;
using MilienAPI.Models.Responses;
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

        public async Task EditProfile(int userId, AccountResponse accountResponse)
        {
            var user = await _unitOfWork.Customers.GetById(userId);

            try
            {
                user.FirstName = accountResponse.FirstName;
                user.LastName = accountResponse.LastName;
                user.AboutMe = accountResponse.AboutMe;
            }
            catch 
            {
                throw new LoginAlreadyExistsException("Логин уже занят друним пользователем!");
            }
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            var customer = await _unitOfWork.Customers.GetById(id);

            return customer;
        }

        public async Task<(Customer, List<Ad>)> GetOwnAds(int userId)
        {
            var user = await _unitOfWork.Customers.GetById(userId);

            var ads = await _unitOfWork.Ads.FindRange(a => a.CustomerId == user.Id);

            var userAds = ads.OrderByDescending(a => a.Id).ToList();
            
            return((user,  userAds));
        }
    }
}
