using Microsoft.AspNetCore.Mvc;
using VacationManager.Companies.Models;

namespace VacationManager.Companies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        /// <summary>
        /// Create a new company. Only accessible to users with the Developer role.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult CreateCompany([FromBody] CompanyCreateModel model)
        {
            // Developer only
            return StatusCode(501); // Not implemented
        }

        /// <summary>
        /// Get a paginated, filtered, and sorted list of companies. Only accessible to CEOs.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetCompanies(
            [FromQuery] string? name,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDir,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            // CEO only
            return StatusCode(501);
        }

        /// <summary>
        /// Get a company by ID. Only accessible to CEOs.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetCompanyById(int id)
        {
            // CEO only
            return StatusCode(501);
        }

        /// <summary>
        /// Update a company's information. Only accessible to CEOs.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult UpdateCompany([FromBody] CompanyUpdateModel model)
        {
            // CEO only
            return StatusCode(501);
        }

        /// <summary>
        /// Delete a company by ID. Only accessible to CEOs.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult DeleteCompany(int id)
        {
            // CEO only
            return StatusCode(501);
        }
    }
}
