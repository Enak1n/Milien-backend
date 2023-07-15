using MilienAPI.Exceptions;
using MilienAPI.Helpers;
using MilienAPI.Models;
using MilienAPI.Models.Requests;
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

        public async Task EditProfile(int userId, AccountRequest accountResponse)
        {
            var user = await _unitOfWork.Customers.GetById(userId);
            string avatarPath = user.Avatar;
            if (accountResponse.Avatar != null)
            {
                avatarPath = FileUploader.UploadImageToServer(accountResponse.Avatar, "/var/avatars");
                if (user.Avatar != null)
                {
                    int lastSlashIndex = user.Avatar.LastIndexOf('/');
                    string path = user.Avatar.Substring(lastSlashIndex + 1);
                    FileUploader.DeleteFileFromServer($"/var/avatars/{path}");
                }
            }

            try
            {
                user.Avatar = avatarPath;
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

            return ((user, userAds));
        }
    }
}
