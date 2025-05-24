using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VacationManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        /// <summary>
        /// Create a new team.Allowed roles: CEO, Manager
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateTeam([FromBody] TeamCreateModel model)
        {
            return StatusCode(501); // Not implemented
        }

        /// <summary>
        /// Get a paginated, filtered, and sorted list of teams.Allowed roles: CEO, Manager
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTeams(
            [FromQuery] string? name,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDir,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            return StatusCode(501);
        }

        /// <summary>
        /// Get a team by ID.Allowed roles: CEO, Manager
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTeamById(int id)
        {
            return StatusCode(501);
        }

        /// <summary>
        /// Update a team.Allowed roles: CEO, Manager
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateTeam([FromBody] TeamUpdateModel model)
        {
            return StatusCode(501);
        }

        /// <summary>
        /// Delete a team by ID. Allowed roles: CEO
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteTeam(int id)
        {
            return StatusCode(501);
        }
    }


    public class TeamCreateModel
    {
        [Required, StringLength(35, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int ManagerId { get; set; }
    }

    public class TeamUpdateModel
    {
        [Required]
        public int Id { get; set; }

        [Required, StringLength(35, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int ManagerId { get; set; }
    }

    public class TeamReadModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ManagerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
