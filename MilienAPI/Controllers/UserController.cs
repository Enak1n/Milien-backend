using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MilienAPI.Models;
using MilienAPI.Models.Responses;
using MilienAPI.Services.Interfaces;

namespace MilienAPI.Controllers
{
    [Route("Customer/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetCustomerById(id);

            if(user == null)
            {
                return BadRequest();
            }

            var dataForAccount = _mapper.Map<Customer, AccountResponse>(user);

            return Ok(dataForAccount);
        }
    }
}
