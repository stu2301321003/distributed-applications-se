using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VacationManager.Auth.Models;
using VacationManager.Companies.Models;
using VacationManager.Companies.Services.Abstractions;

namespace VacationManager.Companies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Dev))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var ceoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var company = await _companyService.CreateAsync(model, ceoId);
            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, company);
        }

        [HttpGet]
        [Authorize(Roles = nameof(Roles.CEO))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetCompanies(
            [FromQuery] string? name,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDir,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var companies = await _companyService.GetAsync(name, sortBy, sortDir, page, pageSize);
            return Ok(companies);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = nameof(Roles.CEO))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            return company == null ? NotFound() : Ok(company);
        }

        [HttpPut]
        [Authorize(Roles = nameof(Roles.CEO))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyUpdateModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var success = await _companyService.UpdateAsync(model);
            return success ? Ok() : NotFound();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = nameof(Roles.CEO))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var success = await _companyService.DeleteAsync(id);
            return success ? Ok() : NotFound();
        }
    }

}
