using Microsoft.EntityFrameworkCore;
using VacationManager.Companies.Entities;
using VacationManager.Companies.Models;
using VacationManager.Companies.Services.Abstractions;
using VacationManager.Database;

namespace VacationManager.Companies.Services.Implementations
{
    public class CompanyService(AppDbContext context) : ICompanyService
    {
        public async Task<Company?> GetByIdAsync(int id)
        {
            return await context.Companies.Include(c => c.Ceo).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<CompanyReadModel>> GetAsync(string? name, string? sortBy, string? sortDir, int page, int pageSize)
        {
            var query = context.Companies.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.Name.Contains(name));

            query = (sortBy?.ToLower(), sortDir?.ToLower()) switch
            {
                ("name", "desc") => query.OrderByDescending(c => c.Name),
                ("name", _) => query.OrderBy(c => c.Name),
                ("createdat", "desc") => query.OrderByDescending(c => c.CreatedAt),
                ("createdat", _) => query.OrderBy(c => c.CreatedAt),
                _ => query.OrderBy(c => c.Id)
            };

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CompanyReadModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    CeoId = c.CeoId,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<Company?> CreateAsync(CompanyCreateModel model, int ceoId)
        {
            var company = new Company
            {
                Name = model.Name,
                CeoId = ceoId,
                CreatedAt = DateTime.UtcNow
            };

            context.Companies.Add(company);
            await context.SaveChangesAsync();
            return company;
        }

        public async Task<bool> UpdateAsync(CompanyUpdateModel model)
        {
            var company = await context.Companies.FindAsync(model.Id);
            if (company == null)
                return false;

            company.Name = model.Name;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var company = await context.Companies.FindAsync(id);
            if (company == null)
                return false;

            context.Companies.Remove(company);
            await context.SaveChangesAsync();
            return true;
        }
    }

}
