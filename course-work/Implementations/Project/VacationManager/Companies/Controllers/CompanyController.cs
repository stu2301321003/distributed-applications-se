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
    public class CompanyController(ICompanyService companyService) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = nameof(Roles.CEO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            int ceoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            Entities.Company? company = await companyService.CreateAsync(model, ceoId);
            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, company);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanies(
            [FromQuery] string? name,
            [FromQuery] string? ceoId,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDir,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var filters = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(name))
                filters["Name"] = name;

            if (!string.IsNullOrWhiteSpace(ceoId))
                filters["CeoId"] = ceoId;

            var result = await companyService.GetAsync(filters, sortBy, sortDir, page, pageSize);
            return Ok(result);
        }



        [HttpGet("{id:int}")]
        [Authorize(Roles = nameof(Roles.CEO))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            Entities.Company? company = await companyService.GetByIdAsync(id);
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
            bool success = await companyService.UpdateAsync(model);
            return success ? Ok() : NotFound();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{nameof(Roles.CEO)},{nameof(Roles.Dev)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            bool success = await companyService.DeleteAsync(id);
            return success ? Ok() : NotFound();
        }

        [HttpGet("company-by-ceo-id/{ceoId:int}")]
        public async Task<IActionResult> GetCompanyByCeoId([FromRoute] int ceoId)
        {
            Entities.Company? company = await companyService.GetByIdAsync(ceoId);
            return company == null ? NotFound() : Ok(company);
        }
    }

}
