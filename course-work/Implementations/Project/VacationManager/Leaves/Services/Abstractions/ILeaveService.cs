using VacationManager.Commons.Enums;
using VacationManager.Leaves.Entities;
using VacationManager.Leaves.Models;

namespace VacationManager.Leaves.Services.Abstractions
{
    public interface ILeaveService
    {
        Task<bool> CreateAsync(LeaveCreateModel model);
        Task<List<Leave>> GetAsync(int? userId, LeaveType? type, string? sortBy, string? sortDir, int page, int pageSize);
        Task<List<Leave>> GetAsync(int? userId);
        Task<Leave?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(LeaveUpdateModel model);
        Task<bool> ApproveAsync(int id);
        Task<bool> RejectAsync(int id, string reason);
        Task<bool> DeleteAsync(int id);
    }
}
