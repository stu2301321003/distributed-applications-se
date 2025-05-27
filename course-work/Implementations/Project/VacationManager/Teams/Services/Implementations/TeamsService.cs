using Microsoft.EntityFrameworkCore;
using VacationManager.Database;
using VacationManager.Teams.Entities;
using VacationManager.Teams.Models;
using VacationManager.Teams.Services.Abstractions;
using VacationManager.Users.Entities;

namespace VacationManager.Teams.Services.Implementations
{
    public class TeamService(AppDbContext context) : ITeamService
    {
        public async Task<TeamReadModel> CreateTeamAsync(TeamCreateModel model)
        {
            Team team = new Team
            {
                Name = model.Name,
                CompanyId = model.CompanyId,
                CreatedAt = DateTime.UtcNow
            };

            context.Teams.Add(team);
            await context.SaveChangesAsync();

            return new TeamReadModel
            {
                Id = team.Id,
                Name = team.Name,
                ManagerId = team.ManagerId,
                CompanyId = team.CompanyId,
                EmployeesCount = 0,
                CreatedAt = team.CreatedAt
            };
        }

        public async Task<List<Team>> GetTeamsAsync(string? name, string? sortBy, string? sortDir, int page, int pageSize)
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

            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Team?> GetTeamByIdAsync(int id)
        {
            var team = await context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null)
                return null;

            team.Employees = await context.Users
                .Where(e => e.TeamId == id)
                .ToListAsync();

            foreach (var item in team.Employees)
            {
                item.TeamId = null;
                item.Team = null;
            }

            return team;
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
        public async Task<bool> SetManagerAsync(int teamId, int userId)
        {
            var team = await context.Teams.Include(t => t.Employees).FirstOrDefaultAsync(t => t.Id == teamId);
            if (team == null) return false;

            // Ensure the user is already part of the team
            if (!team.Employees.Any(e => e.Id == userId))
                return false;

            var user = team.Employees.First(e => e.Id == userId);

            if (user.Role is not Auth.Models.Roles.Employee)
            {
                return false;
            }

            user.Role = Auth.Models.Roles.Manager;
            team.ManagerId = userId;
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveUserFromTeamAsync(int teamId, int userId)
        {
            var team = await context.Teams.Include(t => t.Employees).FirstOrDefaultAsync(t => t.Id == teamId);
            if (team == null) return false;

            var user = team.Employees.FirstOrDefault(e => e.Id == userId);
            if (user == null) return false;

            team.Employees.Remove(user);

            // Also unset as manager if applicable
            if (team.ManagerId == userId)
                team.ManagerId = 0;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddUserToTeamAsync(int teamId, string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            var team = await context.Teams.Include(t => t.Employees).FirstOrDefaultAsync(t => t.Id == teamId);

            if (user == null || team == null)
                return false;

            // Avoid duplicates
            if (team.Employees.Any(e => e.Id == user.Id))
                return false;

            if (user.Role is not Auth.Models.Roles.Unverified or Auth.Models.Roles.Employee)
            {
                return false;
            }

            user.Role = Auth.Models.Roles.Employee;
            team.Employees.Add(user);
            await context.SaveChangesAsync();

            return true;
        }

    }
}
