using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationManager.Users.Entities;
using VacationManager.Users.Models;
using VacationManager.Users.Services.Abstractions;

namespace VacationManager.Users.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [Authorize(Roles = "Employee,Manager,CEO")]
        /// <summary>
        /// Get user by Id. Allowed roles: CEO, Manager, Employee
        /// </summary>
        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Get user by Email. Allowed roles: CEO, Manager
        /// </summary>
        [HttpGet("get-by-mail")]
        public async Task<IActionResult> GetUserByMail([FromQuery] string email)
        {
            var user = await userService.GetUserByEmailAsync(email);
            if (user == null) return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Get user by Phone. Allowed roles: CEO, Manager
        /// </summary>
        [HttpGet("get-by-phone")]
        public async Task<IActionResult> GetUserByPhone([FromQuery] string phone)
        {
            var user = await userService.GetUserByPhoneAsync(phone);
            if (user == null) return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Get paginated list of users. Allowed roles: CEO, Manager
        /// </summary>
        [HttpGet("get")]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1,
                                                  [FromQuery] int pageSize = 10,
                                                  [FromQuery] string sortBy = "Id",
                                                  [FromQuery] string sortDirection = "asc")
        {
            var users = await userService.GetUsersAsync(page, pageSize, sortBy, sortDirection);
            return Ok(users);
        }

        /// <summary>
        /// Update user information. Allowed roles: CEO, Manager, Employee (own profile)
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (user == null) return BadRequest();

            var isSuccess = await userService.UpdateUserAsync(user);
            if (!isSuccess) return BadRequest("User not found or update failed");

            return Ok();
        }

        /// <summary>
        /// Verify a user. Allowed roles: CEO, Manager
        /// </summary>
        [HttpPut("verifyUser")]
        public async Task<IActionResult> VerifyUser([FromBody] VerifyUserModel user)
        {
            if (user == null) return BadRequest();

            var isSuccess = await userService.VerifyUserAsync(user);
            if (!isSuccess) return BadRequest("User not found or verification failed");

            return Ok();
        }

        /// <summary>
        /// Reject a user by Id. Allowed roles: CEO, Manager
        /// </summary>
        [HttpPut("rejectUser/{userId:int}")]
        public async Task<IActionResult> RejectUser([FromRoute] int userId)
        {
            var isSuccess = await userService.RejectUserAsync(userId);
            if (!isSuccess) return BadRequest("User not found or rejection failed");

            return Ok();
        }

        /// <summary>
        /// Delete a user by Id. Allowed roles: CEO, Manager
        /// </summary>
        [HttpDelete("delete/{userId:int}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int userId)
        {
            var isSuccess = await userService.DeleteUserAsync(userId);
            if (!isSuccess) return BadRequest("User not found or deletion failed");

            return Ok();
        }
    }
}
