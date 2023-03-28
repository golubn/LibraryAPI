using Azure.Core;
using LibraryAPI.Domain.Models;
using LibraryAPI.Infrastructure.Services.GuardService;
using LibraryAPI.Infrastructure.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IGuardService _guardService;

        public AuthController(IUserService userService, IGuardService guardService)
        {
            _userService = userService;
            _guardService = guardService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<List<User>>> AddUser(UserRequest user)
        {
            User newUser = new User { UserName = user.UserName, HashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password) };
            var result = await _userService.AddUser(newUser);
            return Ok(result);
        }

        [HttpGet("Check"), Authorize]
        public async Task<ActionResult<List<string>>> CheckToken()
        {
            Request.Headers.TryGetValue("Authorization", out var defaaultValue);
            return Ok(defaaultValue);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> GetOnlyUser(string username, string password)
        {
            var result = await _userService.GetOnlyUser(username, password);
            if (result is null || result.Sessions is null)
            {
                return NotFound("Not Found");
            }
            var term = result.Sessions.Find(el => el.Status == SessionStatus.Open && el.DeletedAt is null);
            if (term is null)
            {
                return NotFound("Not Found 2");
            }

            return Ok(term.Token);
        }

        [HttpPost("Logout"), Authorize]
        public async Task<ActionResult<string>> Logout()
        {
            Request.Headers.TryGetValue("Authorization", out var defaultValue);
            Session session = await _guardService.ValidateSession(defaultValue);
            return Ok(await _userService.Logout(session));
        }

        [HttpPost("WhoAmI"), Authorize]
        public async Task<ActionResult<string>> WhoAmI()
        {
            Request.Headers.TryGetValue("Authorization", out var defaultValue);
            Session session = await _guardService.ValidateSession(defaultValue);
            return Ok(session);
        }


    }
}
