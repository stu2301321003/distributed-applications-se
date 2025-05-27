using Microsoft.EntityFrameworkCore;
using VacationManager.Commons.Enums;
using VacationManager.Database;
using VacationManager.Leaves.Entities;
using VacationManager.Leaves.Models;
using VacationManager.Leaves.Services.Abstractions;

namespace VacationManager.Leaves.Services.Implementations
{
    public class LeaveService(AppDbContext context) : ILeaveService
    {
        public async Task<bool> CreateAsync(LeaveCreateModel model)
        {
            try
            {
                Leave leave = new()
                {
                    From = model.From,
                    To = model.To,
                    UserId = model.UserId,
                    Type = model.Type,
                    Description = model.Description,
                    CreatedAt = DateTime.UtcNow
                };

                context.Leaves.Add(leave);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public async Task<List<Leave>> GetAsync(int? userId, LeaveType? type, string? sortBy, string? sortDir, int page, int pageSize)
        {
            IQueryable<Leave> query = context.Leaves.AsQueryable();
            if (userId.HasValue)
                query = query.Where(l => l.UserId == userId);
            
            if (type.HasValue)
                query = query.Where(l => l.Type == type);
            
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortDir == "desc"
                    ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
                    : query.OrderBy(e => EF.Property<object>(e, sortBy));
            }

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Leave>> GetAsync(int? userId)
        {
            return await context.Leaves.Where(l => l.UserId == userId).ToListAsync();
        }


        public async Task<Leave?> GetByIdAsync(int id)
        {
            return await context.Leaves.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(LeaveUpdateModel model)
        {
            Leave? leave = await context.Leaves.FindAsync(model.Id);
            if (leave == null) return false;
            leave.From = model.From;
            leave.To = model.To;
            leave.UserId = model.UserId;
            leave.Type = model.Type;
            leave.Description = model.Description;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveAsync(int id)
        {
            Leave? leave = await context.Leaves.FindAsync(id);
            if (leave == null) return false;
            leave.IsAccepted = true;
            leave.ResponseMessage = null;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectAsync(int id, string reason)
        {
            Leave? leave = await context.Leaves.FindAsync(id);
            if (leave == null) return false;
            leave.IsAccepted = false;
            leave.ResponseMessage = reason;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Leave? leave = await context.Leaves.FindAsync(id);
            if (leave == null) return false;
            context.Leaves.Remove(leave);
            await context.SaveChangesAsync();
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
