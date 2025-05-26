using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationManager.Auth.Models;
using VacationManager.Teams.Entities;
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

            TeamReadModel team = await teamService.CreateTeamAsync(model);
            return CreatedAtAction(nameof(GetTeamById), new { id = team.Id }, team);
        }

        [HttpGet]
        [Authorize(Roles = nameof(Roles.CEO) + "," + nameof(Roles.Manager))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTeams([FromQuery] string? name, [FromQuery] string? sortBy, [FromQuery] string? sortDir, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            List<Team> teams = await teamService.GetTeamsAsync(name, sortBy, sortDir, page, pageSize);
            return Ok(teams);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = nameof(Roles.CEO) + "," + nameof(Roles.Manager))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeamById(int id)
        {
            Team? team = await teamService.GetTeamByIdAsync(id);
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

            bool updated = await teamService.UpdateTeamAsync(model);
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
            bool deleted = await teamService.DeleteTeamAsync(id);
            if (!deleted)
                return NotFound();

            return Ok("Team deleted successfully.");
        }

        [HttpPost("AddUser")]
        [Authorize(Roles = nameof(Roles.CEO) + "," + nameof(Roles.Manager))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserToTeam([FromBody] AddUserToTeamRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Email is required.");

            bool result = await teamService.AddUserToTeamAsync(request.TeamId, request.Email);

            if (!result)
                return BadRequest("Failed to add user to team. The user may not exist or is already in the team.");

            return Ok("User added to the team successfully.");
        }


        [HttpDelete("{teamId}/remove-user/{userId}")]
        [Authorize(Roles = nameof(Roles.CEO) + "," + nameof(Roles.Manager))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveUserFromTeam(int teamId, int userId)
        {
            bool result = await teamService.RemoveUserFromTeamAsync(teamId, userId);

            if (!result)
                return NotFound("User not found in team or team does not exist.");

            return Ok("User removed from the team successfully.");
        }


        [HttpPut("{teamId}/set-manager/{userId}")]
        [Authorize(Roles = nameof(Roles.CEO))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetManager(int teamId, int userId)
        {
            bool result = await teamService.SetManagerAsync(teamId, userId);

            if (!result)
                return BadRequest("User must be a member of the team to become the manager.");

            return Ok("Manager updated successfully.");
        }


    }

    public class AddUserToTeamRequest
    {
        public int TeamId { get; set; }
        public string Email { get; set; } = string.Empty;
    }

}
