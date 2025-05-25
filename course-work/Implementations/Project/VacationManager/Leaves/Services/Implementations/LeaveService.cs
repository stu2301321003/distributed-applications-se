using Microsoft.EntityFrameworkCore;
using VacationManager.Commons.Enums;
using VacationManager.Database;
using VacationManager.Leaves.Entities;
using VacationManager.Leaves.Models;
using VacationManager.Leaves.Services.Abstractions;

namespace VacationManager.Leaves.Services.Implementations
{
    public class LeaveService : ILeaveService
    {
        private readonly AppDbContext _context;
        public LeaveService(AppDbContext context) => _context = context;

        public async Task<LeaveReadModel> CreateAsync(LeaveCreateModel model)
        {
            var leave = new Leave
            {
                From = model.From,
                To = model.To,
                UserId = model.UserId,
                Type = model.Type,
                Description = model.Description,
                CreatedAt = DateTime.UtcNow
            };
            _context.Leaves.Add(leave);
            await _context.SaveChangesAsync();
            return MapToReadModel(leave);
        }

        public async Task<List<LeaveReadModel>> GetAsync(int? userId, LeaveType? type, string? sortBy, string? sortDir, int page, int pageSize)
        {
            var query = _context.Leaves.AsQueryable();
            if (userId.HasValue) query = query.Where(l => l.UserId == userId);
            if (type.HasValue) query = query.Where(l => l.Type == type);
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortDir == "desc"
                    ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
                    : query.OrderBy(e => EF.Property<object>(e, sortBy));
            }
            return (await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync())
                .Select(MapToReadModel)
                .ToList();
        }

        public async Task<LeaveReadModel?> GetByIdAsync(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);
            return leave == null ? null : MapToReadModel(leave);
        }

        public async Task<bool> UpdateAsync(LeaveUpdateModel model)
        {
            var leave = await _context.Leaves.FindAsync(model.Id);
            if (leave == null) return false;
            leave.From = model.From;
            leave.To = model.To;
            leave.UserId = model.UserId;
            leave.Type = model.Type;
            leave.Description = model.Description;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null) return false;
            leave.IsAccepted = true;
            leave.ResponseMessage = null;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectAsync(int id, string reason)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null) return false;
            leave.IsAccepted = false;
            leave.ResponseMessage = reason;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null) return false;
            _context.Leaves.Remove(leave);
            await _context.SaveChangesAsync();
            return true;
        }

        private static LeaveReadModel MapToReadModel(Leave leave) => new()
        {
            Id = leave.Id,
            From = leave.From,
            To = leave.To,
            UserId = leave.UserId,
            Type = leave.Type,
            Description = leave.Description,
            IsAccepted = leave.IsAccepted,
            ResponseMessage = leave.ResponseMessage,
            CreatedAt = leave.CreatedAt
        };
    }
}
