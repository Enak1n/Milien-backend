using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilienAPI.Exceptions;
using MilienAPI.Models;
using MilienAPI.Models.Responses;
using MilienAPI.Services.Interfaces;
using MilienAPI.UnitOfWork.Interfaces;
using System.Security.Claims;

namespace MilienAPI.Controllers
{
    [Route("Customer/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IMapper mapper, IUserService userService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetCustomerById(id);

            if (user == null)
            {
                return BadRequest();
            }

            var dataForAccount = _mapper.Map<Customer, AccountResponse>(user);

            return Ok(dataForAccount);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOwnAds()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var res = await _userService.GetOwnAds(userId);

            return Ok(new {User = res.Item1, UserAds = res.Item2});
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditProfile(AccountResponse accountResponse)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                await _userService.EditProfile(userId, accountResponse);
                await _unitOfWork.Save();
                return Ok();
            }
            catch (LoginAlreadyExistsException ex) 
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Произошла ошибка при редактировании пользователя");
            }
        }
    }
}
