using VacationManager.Commons.Models;
using VacationManager.Companies.Entities;
using VacationManager.Companies.Models;

namespace VacationManager.Companies.Services.Abstractions
{
    public interface ICompanyService
    {
        Task<Company?> GetByIdAsync(int id);
        Task<Company?> GetByCeoIdAsync(int id);
        Task<PagedResult<CompanyReadModel>> GetAsync(Dictionary<string, string>? filters, string? sortBy, string? sortDir, int page, int pageSize);
        Task<Company?> CreateAsync(CompanyCreateModel model, int ceoId);
        Task<bool> UpdateAsync(CompanyUpdateModel model);
        Task<bool> DeleteAsync(int id);
    }

}
