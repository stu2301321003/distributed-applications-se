using VacationManager.Commons.Enums;
using VacationManager.Leaves.Models;

namespace VacationManager.Leaves.Services.Abstractions
{
    public interface ILeaveService
    {
        Task<LeaveReadModel> CreateAsync(LeaveCreateModel model);
        Task<List<LeaveReadModel>> GetAsync(int? userId, LeaveType? type, string? sortBy, string? sortDir, int page, int pageSize);
        Task<LeaveReadModel?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(LeaveUpdateModel model);
        Task<bool> ApproveAsync(int id);
        Task<bool> RejectAsync(int id, string reason);
        Task<bool> DeleteAsync(int id);
    }
}
