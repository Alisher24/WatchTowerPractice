using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchTower.Database.Models;
using WatchTower.DTO;
using WatchTower.Services;

namespace WatchTower.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(AuthService authService) : ControllerBase
    {
        private readonly AuthService _authService = authService;

        [AllowAnonymous]
        [HttpPost ("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            var loggedInUser = await _authService.Login(user.Email, user.Password);

            return Ok(loggedInUser);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost ("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            User userToRegister = new(user.Name, user.Email, user.Password);

            var registerUser = await _authService.Register(userToRegister);

            return Ok(registerUser);
        }
    }
}
