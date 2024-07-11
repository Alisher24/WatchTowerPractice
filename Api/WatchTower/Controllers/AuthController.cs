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

            var loggedInUser = await authService.Login(user.Name, user.Password);

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

            var registerUser = await authService.Register(userToRegister);

            var loggedInUser = await authService.Login(registerUser.Name, user.Password);

            if (loggedInUser == null)
            {
                return BadRequest(new { message = "Ошибка!" });
            }

            return Ok(loggedInUser);
        }
    }
}
