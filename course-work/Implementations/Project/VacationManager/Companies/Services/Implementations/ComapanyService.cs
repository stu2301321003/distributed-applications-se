using Microsoft.EntityFrameworkCore;
using VacationManager.Commons.Models;
using VacationManager.Companies.Entities;
using VacationManager.Companies.Models;
using VacationManager.Companies.Services.Abstractions;
using VacationManager.Database;
using System.Linq.Dynamic.Core;

namespace VacationManager.Companies.Services.Implementations
{
    public class CompanyService(AppDbContext context) : ICompanyService
    {
        public async Task<Company?> GetByIdAsync(int id)
        {
            return await context.Companies.Include(c => c.Ceo).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Company?> GetByCeoIdAsync(int id)
        {
            return await context.Companies.Include(c => c.Ceo).FirstOrDefaultAsync(c => c.Ceo.Id == id);
        }

        public async Task<Commons.Models.PagedResult<CompanyReadModel>> GetAsync(
            Dictionary<string, string>? filters,
            string? sortBy,
            string? sortDir,
            int page,
            int pageSize)
        {
            IQueryable<Company> query = context.Companies.AsQueryable();

            // Apply filters dynamically
            if (filters is not null)
            {
                foreach (var filter in filters)
                {
                    if (!string.IsNullOrWhiteSpace(filter.Value))
                    {
                        // Supports partial match (LIKE '%value%') for strings
                        query = query.Where($"{filter.Key}.Contains(@0)", filter.Value);
                    }
                }
            }

            var totalCount = await query.CountAsync();

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var direction = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase) ? "descending" : "ascending";
                query = query.OrderBy($"{sortBy} {direction}");
            }
            else
            {
                query = query.OrderBy(c => c.Id);
            }

            // Apply paging
            var items = await query
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

            return new Commons.Models.PagedResult<CompanyReadModel> { Items = items, TotalCount = totalCount };
        }


        public async Task<Company?> CreateAsync(CompanyCreateModel model, int ceoId)
        {
            Company company = new Company
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
            Company? company = await context.Companies.FindAsync(model.Id);
            if (company == null)
                return false;

            company.Name = model.Name;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Company? company = await context.Companies.FindAsync(id);
            if (company == null)
                return false;

            context.Companies.Remove(company);
            await context.SaveChangesAsync();
            return true;
        }
    }

}
