using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VacationManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Get user by Id. Allowed roles: CEO, Manager, Employee
        /// </summary>
        [HttpGet("get/{id:int}")]
        public IActionResult GetUser([FromRoute] int id)
        {
            bool isSuccess = true;
            return isSuccess ? Ok(new User()) : NotFound();
        }

        /// <summary>
        /// Get user by Email. Allowed roles: CEO, Manager
        /// </summary>
        [HttpGet("get-by-mail/")]
        public IActionResult GetUserByMail([FromQuery] string email)
        {
            bool isSuccess = true;
            return isSuccess ? Ok(new User()) : NotFound();
        }

        /// <summary>
        /// Get user by Phone. Allowed roles: CEO, Manager
        /// </summary>
        [HttpGet("get-by-phone/")]
        public IActionResult GetUserByPhone([FromQuery] string phone)
        {
            bool isSuccess = true;
            return isSuccess ? Ok(new User()) : NotFound();
        }

        /// <summary>
        /// Get paginated list of users. Allowed roles: CEO, Manager
        /// </summary>
        [HttpGet("get/")]
        public IActionResult GetUsers([FromQuery] int page = 1,
                                      [FromQuery] int pageSize = 10,
                                      [FromQuery] string sortBy = "Id",
                                      [FromQuery] string sortDirection = "asc")
        {
            return Ok(new List<User>());
        }

        /// <summary>
        /// Update user information. Allowed roles: CEO, Manager, Employee (own profile)
        /// </summary>
        [HttpPut("update/")]
        public IActionResult GetUser([FromQuery] User user)
        {
            bool isSuccess = true;
            return isSuccess ? Ok() : BadRequest();
        }

        /// <summary>
        /// Verify a user. Allowed roles: CEO, Manager
        /// </summary>
        [HttpPut("verifyUser/")]
        public IActionResult VerifyUser([FromQuery] VerifyUserModel user)
        {
            bool isSuccess = true;
            return isSuccess ? Ok() : BadRequest();
        }

        /// <summary>
        /// Reject a user by Id. Allowed roles: CEO, Manager
        /// </summary>
        [HttpPut("rejectUser/{userId:int}")]
        public IActionResult RejectUser([FromRoute] int userId)
        {
            bool isSuccess = true;
            return isSuccess ? Ok() : BadRequest();
        }

        /// <summary>
        /// Delete a user by Id. Allowed roles: CEO, Manager
        /// </summary>
        [HttpDelete("delete/{userId:int}")]
        public IActionResult DeleteUser([FromRoute] int userId)
        {
            bool isSuccess = true;
            return isSuccess ? Ok() : BadRequest();
        }
    }


    public class UpdateUser
    {
        [Phone, Length(5, 20)]
        public string PhoneNumber { get; set; } = null!;

        [Required, Length(5, 50)]
        public string Password { get; set; } = null!;
    }

    public class VerifyUserModel
    {
        [EmailAddress, Length(5, 50)]
        public string Email { get; set; } = null!;

        [Phone, Length(5, 20)]
        public string PhoneNumber { get; set; } = null!;

        public Roles Role { get; set; }
        public int? TeamId { get; set; }
    }
}
