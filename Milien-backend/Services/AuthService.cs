using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Millien.Domain.DataBase;
using Millien.Domain.Entities;
using Milien_backend.Models.Requsets;
using Milien_backend.Models.Responses;
using Milien_backend.Services.Interfaces;
using System.Security.Claims;

namespace Milien_backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly Context _context;
        private readonly ITokenService _tokenService;
        private PasswordHasher<string> _passwordHasher = new();
        private IMapper _mapper;

        public AuthService(Context context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<AuthenticateResponse> Login(LoginRequest loginRequest)
        {
            Customer user = await _context.Customers.FirstOrDefaultAsync(c => c.Login == loginRequest.Login || c.PhoneNumber == loginRequest.Login);
            var userFromLogin = await _context.Login.FirstOrDefaultAsync(u => u.Login == user.Login);
            if (user == null)
            {
                throw new Exception("Неверный логин или пароль!");
            }
            var passwordResult = _passwordHasher.VerifyHashedPassword(null, userFromLogin.Password, loginRequest.Password);

            if (passwordResult != PasswordVerificationResult.Success)
                throw new Exception("Неверный логин или пароль!");


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginRequest.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            userFromLogin.RefreshToken = refreshToken;
            userFromLogin.RefreshTokenExpiryTime = DateTime.Now.AddDays(15);

            await _context.SaveChangesAsync();

            return new AuthenticateResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserDTO = user
            };
        }

        public async Task Register(UserRequest userRequest)
        {
            string pass = _passwordHasher.HashPassword(null, userRequest.Pass);
            userRequest.Pass = pass;

            LoginModel login = new LoginModel
            {
                Login = userRequest.Login,
                Password = userRequest.Pass
            };

            var createdUser = _mapper.Map<UserRequest, Customer>(userRequest);
            createdUser.Role = Role.User;
            createdUser.ComfimedEmail = true;
            _context.Customers.Add(createdUser);
            _context.Login.Add(login);
            await _context.SaveChangesAsync();
        }

        public async Task ResetPassword(string phone)
        {
            var user = await _context.Customers.Where(u => u.PhoneNumber == phone).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("Пользователя с таким номером не существует!");
            }
        }


        public async Task CreateNewPassword(string password, string phone)
        {
            var user = await _context.Customers.Where(u => u.PhoneNumber == phone).FirstOrDefaultAsync();
            var login = await _context.Login.Where(l => l.Login == user.Login).FirstOrDefaultAsync();

            string passForCustomer = _passwordHasher.HashPassword(null, password);
            user.Pass = passForCustomer;

            login.Password = passForCustomer;

            await _context.SaveChangesAsync();
        }

        public async Task ConfirmEmail(string code, string login)
        {
            var userForCheck = await _context.Customers.FirstOrDefaultAsync(c => c.Login == login);

            if (userForCheck != null && userForCheck.ConfirmedCode == code)
            {
                userForCheck.ComfimedEmail = true;
                userForCheck.ConfirmedCode = null;

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Неверный код подтверждения!");
            }
        }
    }
}
