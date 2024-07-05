using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WatchTower.Server.BusinessLogic.Interface;
using WatchTower.Server.DTO;
using WatchTower.Server.Models;

namespace WatchTower.Server.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost ("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                return BadRequest(new { message = "Введите Имя" });
            }

            else if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { message = "Введите пароль" });
            }

            User loggedInUser = await _authService.Login(user.Name, user.Password);

            if (loggedInUser == null)
            {
                return BadRequest(new { message = "Данный пользователь не найден" });
            }

            return Ok(loggedInUser);
        }

        [AllowAnonymous]
        [HttpPost ("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                return BadRequest(new { message = "Введите имя" });
            }

            else if (string.IsNullOrEmpty(user.Email))
            {
                return BadRequest(new { message = "Введите E-mail" });
            }

            else if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { message = "Введите пароль" });
            }

            User userToRegister = new(name: user.Name, email: user.Email, password: user.Password);

            User registerUser = await _authService.Register(userToRegister);

            User loggedInUser = await _authService.Login(registerUser.Name, user.Password);

            if (loggedInUser == null)
            {
                return BadRequest(new { message = "Ошибка!" });
            }

            return Ok(loggedInUser);
        }

        [Authorize(Roles = "Everyone")]
        [HttpGet]
        public IActionResult Test()
        {
            string token = Request.Headers["Authorization"];

            if (token.StartsWith("Bearer"))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            var handler = new JwtSecurityTokenHandler();

            JwtSecurityToken jwt = handler.ReadJwtToken(token);

            var claims = new Dictionary<string, string>();

            foreach (var claim in jwt.Claims)
            {
                claims.Add(claim.Type, claim.Value);
            }

            return Ok(claims);
        }
    }
}
