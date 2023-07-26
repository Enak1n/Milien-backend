using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Milien_backend.Models.Enums;
using Milien_backend.Models.Responses;
using Milien_backend.Services.Interfaces;
using Millien.Domain.DataBase;
using System.Net;

namespace Milien_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly Context _context;
        private readonly ITokenService _tokenService;

        public TokenController(Context context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(TokenApiModel tokenApiModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid client request");

            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;

            var currentIdentityUser = _context.Login.SingleOrDefault(u => u.Login == username);

            if (currentIdentityUser is null || currentIdentityUser.RefreshToken != refreshToken
                 || currentIdentityUser.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return Unauthorized();
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            currentIdentityUser.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();

            return Ok(new AuthenticateResponse()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }

        [HttpPost]
        [Route("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var currentIdentityUser = User.Identity.Name;

            var loginedUser = await _context.Login.SingleOrDefaultAsync(u => u.Login == currentIdentityUser);

            if (loginedUser is null)
            {
                return BadRequest();
            }

            loginedUser.RefreshToken = null;
            loginedUser.RefreshTokenExpiryTime = null;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
