using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using VacationManager.Auth.Helpers;
using VacationManager.Auth.Models;
using VacationManager.Database;
using VacationManager.Users.Entities;
using VacationManager.Users.Services.Abstractions;

namespace VacationManager.Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(ILogger<AuthController> logger, IUserService userService, IOptions<JwtSettings> options) : ControllerBase
    {
        /// <summary>
        /// Login request enabled for not authorized users
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn([FromBody] UserLoginModel loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
                return BadRequest("Invalid login data.");

            // Find user by email or username (assuming Username is email here)
            User? user = await userService.GetUserByEmailAsync(loginModel.Username);
            if (user == null)
            {
                logger.LogWarning("Login failed: user not found with username {Username}", loginModel.Username);
                return Unauthorized();
            }

            string hashedInputPassword = PasswordHelper.HashPassword(loginModel.Password, user.Salt);
            if (user.Password != hashedInputPassword)
            {
                logger.LogWarning("Login failed: invalid password for user {Username}", loginModel.Username);
                return Unauthorized();
            }


            // TODO: Generate JWT or session token here
            string token = JwtHelper.GenerateJwtToken(user, options.Value);
            return Ok(new { Message = "Login successful", Token = token });
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel userRegisterModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if user with email already exists
            User? existingUser = await userService.GetUserByEmailAsync(userRegisterModel.Email);
            if (existingUser != null)
            {
                logger.LogWarning("Registration failed: email {Email} already in use", userRegisterModel.Email);
                return Conflict("Email already in use.");
            }

            // Inside Register method
            string salt = PasswordHelper.GenerateSalt();
            string hashedPassword = PasswordHelper.HashPassword(userRegisterModel.Password, salt);

            User newUser = new()
            {
                Name = userRegisterModel.Name,
                LastName = userRegisterModel.LastName,
                Email = userRegisterModel.Email,
                PhoneNumber = userRegisterModel.PhoneNumber,
                Password = hashedPassword,
                Salt = salt,
                Role = Roles.Unverified,
                CreatedAt = DateTime.UtcNow
            };

            // Add user to database (implement AddUserAsync in your service)
            bool isAdded = await userService.AddUserAsync(newUser);
            if (!isAdded)
            {
                logger.LogError("Registration failed for email {Email}", userRegisterModel.Email);
                return StatusCode(500, "User registration failed.");
            }

            logger.LogInformation("User registered successfully with email {Email}", userRegisterModel.Email);
            return Ok("Registration successful");
        }
    }

}
