using Milien_backend.Models.Requsets;
using Milien_backend.Models.Responses;

namespace Milien_backend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticateResponse> Login(LoginRequest loginRequest);
        Task Register(UserRequest userRequest);
        Task ResetPassword(string email);
        Task CreateNewPassword(string password, string email);
        Task ConfirmEmail(string code, string email);
        Task SendEmail(string login);
    }
}
