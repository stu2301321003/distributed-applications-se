using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationManager.Auth.Models;
using VacationManager.Teams.Models;
using VacationManager.Teams.Services.Abstractions;

namespace VacationManager.Teams.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController(ITeamService teamService, ILogger<TeamsController> logger) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = nameof(Roles.CEO) + "," + nameof(Roles.Manager))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTeam([FromBody] TeamCreateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var team = await teamService.CreateTeamAsync(model);
            return CreatedAtAction(nameof(GetTeamById), new { id = team.Id }, team);
        }

        [HttpGet]
        [Authorize(Roles = nameof(Roles.CEO) + "," + nameof(Roles.Manager))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTeams([FromQuery] string? name, [FromQuery] string? sortBy, [FromQuery] string? sortDir, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var teams = await teamService.GetTeamsAsync(name, sortBy, sortDir, page, pageSize);
            return Ok(teams);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = nameof(Roles.CEO) + "," + nameof(Roles.Manager))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeamById(int id)
        {
            var team = await teamService.GetTeamByIdAsync(id);
            if (team == null)
                return NotFound();
            return Ok(team);
        }

        [HttpPut]
        [Authorize(Roles = nameof(Roles.CEO) + "," + nameof(Roles.Manager))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTeam([FromBody] TeamUpdateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await teamService.UpdateTeamAsync(model);
            if (!updated)
                return NotFound();

            return Ok("Team updated successfully.");
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = nameof(Roles.CEO))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var deleted = await teamService.DeleteTeamAsync(id);
            if (!deleted)
                return NotFound();

            return Ok("Team deleted successfully.");
        }
    }
}
