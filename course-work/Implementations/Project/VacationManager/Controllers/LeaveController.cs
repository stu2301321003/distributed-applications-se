using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VacationManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeavesController : ControllerBase
    {
        /// <summary>
        /// Create a new leave. Allowed roles: Manager, Employee
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateLeave([FromBody] LeaveCreateModel model)
        {
            return StatusCode(501); // Not implemented
        }

        /// <summary>
        /// Get a paginated, filtered, and sorted list of leaves.
        /// Filtering by UserId and Type possible. Allowed roles: CEO, Manager
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetLeaves(
            [FromQuery] int? userId,
            [FromQuery] LeaveType? type,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDir,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            return StatusCode(501);
        }

        /// <summary>
        /// Get a leave by ID. Allowed roles: CEO, Manager, Employee
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetLeaveById(int id)
        {
            return StatusCode(501);
        }

        /// <summary>
        /// Update a leave. Allowed roles: Manager, Employee
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateLeave([FromBody] LeaveUpdateModel model)
        {
            return StatusCode(501);
        }

        /// <summary>
        /// Approves a leave. Allowed roles: CEO, Manager
        /// </summary>
        [HttpPut("approve/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ApproveLeave([FromRoute] int id)
        {
            return StatusCode(501);
        }

        /// <summary>
        /// Rejects a leave.Allowed roles: CEO, Manager
        /// </summary>
        [HttpPut("reject/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RejectLeave([FromRoute] int id)
        {
            return StatusCode(501);
        }

        /// <summary>
        /// Delete a leave by ID. Allowed roles: Manager, Employee
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteLeave(int id)
        {
            return StatusCode(501);
        }
    }


    public class LeaveCreateModel
    {
        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public LeaveType Type { get; set; }
    }

    public class LeaveUpdateModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public LeaveType Type { get; set; }
    }

    public class LeaveReadModel
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int UserId { get; set; }
        public LeaveType Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
