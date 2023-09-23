using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Milien_backend.Models.Requsets;
using Milien_backend.Services.Interfaces;
using Millien.Domain.DataBase;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;
using System.Net;
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
        [Route("check_phone")]
        public async Task<IActionResult> CheckPhone(string phoneNumber)
        {
            string updatedNumber = "8" + phoneNumber.Substring(1);
            if (await _availabilityService.CheckPhoneNumber(updatedNumber))
            {
                var options = new RestClientOptions("https://lite-mobileid.beeline.ru")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/lite-auth", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Basic TWlsbGlvbl9MaXRlQXBpOkZaaEhXRDQ0dk9kTzNuVVM=");
                var body = @"{
                            " + "\n" +
                                @"    ""response_type"": ""polling"",
                            " + "\n" +
                                            $@"    ""msisdn"": ""{phoneNumber}""
                            " + "\n" +
                @"}";
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = await client.ExecuteAsync(request);

                return Ok(response.Content);
            }
            else
            {
                return BadRequest("Аккаунт с таким номером уже зарегистрирован!");
            }
        }

        [HttpGet]
        [Route("check_authMobilePhone")]
        public async Task<IActionResult> CheckAuthMobilePhone(string requestId)
        {
            var options = new RestClientOptions("https://lite-mobileid.beeline.ru")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/lite-result", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Basic TWlsbGlvbl9MaXRlQXBpOkZaaEhXRDQ0dk9kTzNuVVM=");
            var body = @"{
                            " + "\n" +
                                        $@"    ""request_id"": ""{requestId}""
                            " + "\n" +
            @"}";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);

            return Ok(response.Content);
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

                CookieOptions cookiie = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7)
                };

                Response.Cookies.Append("userInfo", res.RefreshToken, cookiie);
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

                string phoneNumber = '7' + email.Substring(1);
                var options = new RestClientOptions("https://lite-mobileid.beeline.ru")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/lite-auth", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Basic TWlsbGlvbl9MaXRlQXBpOkZaaEhXRDQ0dk9kTzNuVVM=");
                var body = @"{
                            " + "\n" +
                                @"    ""response_type"": ""polling"",
                            " + "\n" +
                                            $@"    ""msisdn"": ""{phoneNumber}""
                            " + "\n" +
                @"}";
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = await client.ExecuteAsync(request);

                return Ok(response.Content);
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
