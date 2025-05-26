using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationManager.Auth.Models;
using VacationManager.Commons.Enums;
using VacationManager.Leaves.Models;
using VacationManager.Leaves.Services.Abstractions;

namespace VacationManager.Leaves.Controllers
{
    public class LeavesController(ILeaveService leavesService) : ControllerBase
    {

        /// <summary>
        /// Create a new leave. Allowed roles: Manager, Employee
        /// </summary>
        [HttpPost]
        [Authorize(Roles = $"{nameof(Roles.Manager)},{nameof(Roles.Employee)}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateLeave([FromBody] LeaveCreateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Task<LeaveReadModel> createdLeave = leavesService.CreateAsync(model);
            return CreatedAtAction(nameof(GetLeaveById), new { id = createdLeave.Id }, createdLeave);
        }

        /// <summary>
        /// Get a paginated, filtered, and sorted list of leaves. Filtering by UserId and Type possible. Allowed roles: CEO, Manager
        /// </summary>
        [HttpGet]
        [Authorize(Roles = $"{nameof(Roles.CEO)},{nameof(Roles.Manager)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLeaves(
            [FromQuery] int? userId,
            [FromQuery] LeaveType? type,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDir,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            List<LeaveReadModel> leaves = await leavesService.GetAsync(userId, type, sortBy, sortDir, page, pageSize);
            return Ok(leaves);
        }

        /// <summary>
        /// Get a leave by ID. Allowed roles: CEO, Manager, Employee
        /// </summary>
        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{nameof(Roles.CEO)},{nameof(Roles.Manager)},{nameof(Roles.Employee)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetLeaveById(int id)
        {
            Task<LeaveReadModel?> leave = leavesService.GetByIdAsync(id);
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
            if (string.IsNullOrWhiteSpace(message))
                return BadRequest("Rejection message is required.");

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
