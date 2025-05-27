using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VacationManager.Auth.Models;
using VacationManager.Commons.Enums;
using VacationManager.Leaves.Entities;
using VacationManager.Leaves.Models;
using VacationManager.Leaves.Services.Abstractions;

namespace VacationManager.Leaves.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeavesController(ILeaveService leavesService) : ControllerBase
    {

        /// <summary>
        /// Create a new leave. Allowed roles: Manager, Employee
        /// </summary>
        [HttpPost]
        [Authorize(Roles = $"{nameof(Roles.Manager)},{nameof(Roles.Employee)}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <IActionResult> CreateLeave([FromBody] LeaveCreateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool isSuccessful = await leavesService.CreateAsync(model);
            return isSuccessful ? Ok() : StatusCode(500, "Something went wrong");
        }

        /// <summary>
        /// Get a paginated, filtered, and sorted list of leaves. Filtering by UserId and Type possible. Allowed roles: CEO, Manager
        /// </summary>
        [HttpGet]
        [Authorize(Roles = $"{nameof(Roles.CEO)},{nameof(Roles.Manager)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLeaves(
    [FromQuery] LeaveType? type,
    [FromQuery] string? sortBy,
    [FromQuery] string? sortDir,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (userIdClaim == null || roleClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var leaves = await leavesService.GetFilteredLeavesAsync(userId, roleClaim, type, sortBy, sortDir, page, pageSize);
            return Ok(leaves);
        }


        [HttpGet("all/{userId:int}")]
        [Authorize(Roles = $"{nameof(Roles.Employee)},{nameof(Roles.Manager)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLeaves(int userId)
        {
            List<Leave> leaves = await leavesService.GetAsync(userId);
            return Ok(leaves);
        }


        /// <summary>
        /// Get a leave by ID. Allowed roles: CEO, Manager, Employee
        /// </summary>
        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{nameof(Roles.CEO)},{nameof(Roles.Manager)},{nameof(Roles.Employee)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLeaveById(int id)
        {
            Leave? leave = await leavesService.GetByIdAsync(id);
            return leave == null ? NotFound() : Ok(leave);
        }

        /// <summary>
        /// Update a leave. Allowed roles: Manager, Employee
        /// </summary>
        [HttpPut]
        [Authorize(Roles = $"{nameof(Roles.Manager)},{nameof(Roles.Employee)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLeave([FromBody] LeaveUpdateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool success = await leavesService.UpdateAsync(model);
            return success ? Ok() : NotFound();
        }

        /// <summary>
        /// Approves a leave. Allowed roles: CEO, Manager
        /// </summary>
        [HttpPut("approve/{id:int}")]
        [Authorize(Roles = $"{nameof(Roles.CEO)},{nameof(Roles.Manager)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApproveLeave([FromRoute] int id)
        {
            bool success = await leavesService.ApproveAsync(id);
            return success ? Ok() : NotFound();
        }

        /// <summary>
        /// Rejects a leave. Allowed roles: CEO, Manager
        /// </summary>
        [HttpPut("reject/{id:int}")]
        [Authorize(Roles = $"{nameof(Roles.CEO)},{nameof(Roles.Manager)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RejectLeave([FromRoute] int id, [FromQuery] string message)
        {
            bool success = await leavesService.RejectAsync(id, message);
            return success ? Ok() : NotFound();
        }

        /// <summary>
        /// Delete a leave by ID. Allowed roles: Manager, Employee
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{nameof(Roles.Manager)},{nameof(Roles.Employee)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLeave(int id)
        {
            bool success = await leavesService.DeleteAsync(id);
            return success ? Ok() : NotFound();
        }
    }
}
