using Microsoft.AspNetCore.Mvc;
using Milien_backend.Models.Requsets;
using Milien_backend.Services.Interfaces;

namespace Milien_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAvailabilityService _availabilityService;

        public AuthController(IAuthService authService, IAvailabilityService availabilityService)
        {
            _authService = authService;
            _availabilityService = availabilityService;
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
        public async Task<IActionResult> ConfirmEmail(string code, string email)
        {
            try
            {
                await _authService.ConfirmEmail(code, email);
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
    }
}
