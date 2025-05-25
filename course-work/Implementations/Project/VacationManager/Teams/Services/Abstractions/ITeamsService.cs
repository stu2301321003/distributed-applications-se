using VacationManager.Teams.Models;

namespace VacationManager.Teams.Services.Abstractions
{
    public interface ITeamService
    {
        Task<TeamReadModel> CreateTeamAsync(TeamCreateModel model);
        Task<List<TeamReadModel>> GetTeamsAsync(string? name, string? sortBy, string? sortDir, int page, int pageSize);
        Task<TeamReadModel?> GetTeamByIdAsync(int id);
        Task<bool> UpdateTeamAsync(TeamUpdateModel model);
        Task<bool> DeleteTeamAsync(int id);
    }
}
