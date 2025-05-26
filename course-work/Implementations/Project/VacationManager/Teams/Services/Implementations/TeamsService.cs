using Microsoft.EntityFrameworkCore;
using VacationManager.Database;
using VacationManager.Teams.Entities;
using VacationManager.Teams.Models;
using VacationManager.Teams.Services.Abstractions;

namespace VacationManager.Teams.Services.Implementations
{
    public class TeamService(AppDbContext context) : ITeamService
    {
        public async Task<TeamReadModel> CreateTeamAsync(TeamCreateModel model)
        {
            Team team = new Team
            {
                Name = model.Name,
                ManagerId = model.ManagerId,
                CreatedAt = DateTime.UtcNow
            };

            context.Teams.Add(team);
            await context.SaveChangesAsync();

            return new TeamReadModel
            {
                Id = team.Id,
                Name = team.Name,
                ManagerId = team.ManagerId,
                CreatedAt = team.CreatedAt
            };
        }

        public async Task<List<TeamReadModel>> GetTeamsAsync(string? name, string? sortBy, string? sortDir, int page, int pageSize)
        {
            IQueryable<Team> query = context.Teams.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(t => t.Name.Contains(name));

            query = (sortBy?.ToLower(), sortDir?.ToLower()) switch
            {
                ("name", "desc") => query.OrderByDescending(t => t.Name),
                ("name", _) => query.OrderBy(t => t.Name),
                ("createdat", "desc") => query.OrderByDescending(t => t.CreatedAt),
                ("createdat", _) => query.OrderBy(t => t.CreatedAt),
                _ => query.OrderBy(t => t.Id)
            };

            List<Team> teams = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return teams.Select(t => new TeamReadModel
            {
                Id = t.Id,
                Name = t.Name,
                ManagerId = t.ManagerId,
                CreatedAt = t.CreatedAt
            }).ToList();
        }

        public async Task<TeamReadModel?> GetTeamByIdAsync(int id)
        {
            Team? team = await context.Teams.FindAsync(id);
            if (team == null) return null;

            return new TeamReadModel
            {
                Id = team.Id,
                Name = team.Name,
                ManagerId = team.ManagerId,
                CreatedAt = team.CreatedAt
            };
        }

        public async Task<bool> UpdateTeamAsync(TeamUpdateModel model)
        {
            Team? team = await context.Teams.FindAsync(model.Id);
            if (team == null) return false;

            team.Name = model.Name;
            team.ManagerId = model.ManagerId;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTeamAsync(int id)
        {
            Team? team = await context.Teams.FindAsync(id);
            if (team == null) return false;

            context.Teams.Remove(team);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
