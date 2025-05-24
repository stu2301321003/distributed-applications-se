using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VacationManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Auth : ControllerBase
    {
        private readonly ILogger<Auth> _logger;

        public Auth(ILogger<Auth> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Login requiest enabled for not authorixed users
        /// </summary>
        [HttpPost("login")]
        public IActionResult LogIn([FromQuery] UserLoginModel loginModel)
        {
            bool isSuccess = true;
            return isSuccess ? Ok(loginModel) : Unauthorized();
        }

        /// <summary>
        /// Login requiest enabled for not authorixed users
        /// </summary>
        [HttpPost("register"), AllowAnonymous]
        public IActionResult LogOut([FromQuery] UserRegisterModel userRegisterModel)
        {
            bool isSuccess = true;
            return isSuccess ? Ok() : Conflict();
        }
    }

    public class UserLoginModel()
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserRegisterModel()
    {
        [Required, Length(2, 25)]
        public string Name { get; set; } = null!;

        [Required, Length(2, 35)]
        public string LastName { get; set; } = null!;

        [EmailAddress, Length(5, 50)]
        public string Email { get; set; } = null!;

        [Phone, Length(5, 20)]
        public string PhoneNumber { get; set; } = null!;

        [Required, Length(5, 50)]
        public string Password { get; set; } = null!;
    }
}
