using VacationManager.Companies.Entities;
using VacationManager.Companies.Models;

namespace VacationManager.Companies.Services.Abstractions
{
    public interface ICompanyService
    {
        Task<Company?> GetByIdAsync(int id);
        Task<List<CompanyReadModel>> GetAsync(string? name, string? sortBy, string? sortDir, int page, int pageSize);
        Task<Company?> CreateAsync(CompanyCreateModel model, int ceoId);
        Task<bool> UpdateAsync(CompanyUpdateModel model);
        Task<bool> DeleteAsync(int id);
    }

}
