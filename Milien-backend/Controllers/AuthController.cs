using Microsoft.AspNetCore.Mvc;
using Milien_backend.Models.Requsets;
using Milien_backend.Services.Interfaces;
using Millien.Domain.DataBase;
using System.Xml.Linq;

namespace Milien_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAvailabilityService _availabilityService;
        private readonly Context _context;

        public AuthController(IAuthService authService, IAvailabilityService availabilityService, Context context)
        {
            _authService = authService;
            _availabilityService = availabilityService;
            _context = context;
        }

        [HttpGet]
        [Route("check_accept_email")]
        public async Task<IActionResult> CheckAcceptEmail(string login)
        {
            var user = _context.Customers.Where(u => u.Login == login).FirstOrDefault();
            if (user == null)
            {
                return NotFound("Неверный логин или пароль!");
            }

            return Ok(await _availabilityService.CheckAcceptEmail(login));
        }

        [HttpGet]
        [Route("check_code")]
        public async Task<bool> CheckCode(string code, string email)
        {
            return await _availabilityService.CheckCode(code, email);
        }

        [HttpGet]
        [Route("check_email")]
        public async Task<bool> CheckEmail(string email)
        {
            return await _availabilityService.CheckEmail(email);
        }

        [HttpGet]
        [Route("check_phone")]
        public async Task<bool> CheckPhone(string phoneNumber)
        {
            return await _availabilityService.CheckPhoneNumber(phoneNumber);
        }

        [HttpGet]
        [Route("check_login")]
        public async Task<bool> CheckLogin(string login)
        {
            return await _availabilityService.CheckLogin(login);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string code, string login)
        {
            try
            {
                await _authService.ConfirmEmail(code, login);
                return Ok("Почта подтверждена!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var res = await _authService.Login(loginRequest);

                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                    IsEssential = true
                };

                Response.Cookies.Append("Name", res.RefreshToken, options);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost, Route("register")]
        public async Task<IActionResult> Register(UserRequest userRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Something went wrong!");

            try
            {
                await _authService.Register(userRequest);
                return Content("Для завершения регистрации проверьте почту!");
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }

        [HttpPut]
        [Route("reset_password")]
        public async Task<IActionResult> ResetPassword(string email)
        {
            try
            {
                await _authService.ResetPassword(email);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("create_new_password")]
        public async Task<IActionResult> CreateNewPassword(string password, string email)
        {
            await _authService.CreateNewPassword(password, email);
            return Ok();
        }

        [HttpGet]
        [Route("send_email")]
        public async Task<IActionResult> SendEmail(string login)
        {
            try
            {
                await _authService.SendEmail(login);
                return Ok();
            }
            catch
            {
                return BadRequest("Ошибка при отправке письма!");
            }
        }
    }
}
