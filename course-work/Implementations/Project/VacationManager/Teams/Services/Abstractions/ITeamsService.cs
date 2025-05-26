using VacationManager.Teams.Entities;
using VacationManager.Teams.Models;

namespace VacationManager.Teams.Services.Abstractions
{
    public interface ITeamService
    {
        Task<TeamReadModel> CreateTeamAsync(TeamCreateModel model);
        Task<List<Team>> GetTeamsAsync(string? name, string? sortBy, string? sortDir, int page, int pageSize);
        Task<Team?> GetTeamByIdAsync(int id);
        Task<bool> UpdateTeamAsync(TeamUpdateModel model);
        Task<bool> DeleteTeamAsync(int id);
        Task<bool> AddUserToTeamAsync(int teamId, string email);
        Task<bool> RemoveUserFromTeamAsync(int teamId, int userId);
        Task<bool> SetManagerAsync(int teamId, int userId);

    }
}
