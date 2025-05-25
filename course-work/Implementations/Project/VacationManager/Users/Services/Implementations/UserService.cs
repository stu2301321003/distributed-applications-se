using Microsoft.EntityFrameworkCore;
using VacationManager.Auth.Models;
using VacationManager.Database;
using VacationManager.Users.Entities;
using VacationManager.Users.Models;
using VacationManager.Users.Services.Abstractions;

namespace VacationManager.Users.Services.Implementations
{
    public class UserService(AppDbContext context) : IUserService
    {
        public async Task<User?> GetUserByIdAsync(int id) => await context.Users.FindAsync(id);

        public Task<User?> GetUserByEmailAsync(string email) => context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public Task<User?> GetUserByPhoneAsync(string phone) => context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phone);

        public async Task<IList<User>> GetUsersAsync(int page, int pageSize, string sortBy, string sortDirection)
        {
            IQueryable<User> query = context.Users;

            query = (sortBy.ToLower(), sortDirection.ToLower()) switch
            {
                ("name", "asc") => query.OrderBy(u => u.Name),
                ("name", "desc") => query.OrderByDescending(u => u.Name),
                ("lastname", "asc") => query.OrderBy(u => u.LastName),
                ("lastname", "desc") => query.OrderByDescending(u => u.LastName),
                ("email", "asc") => query.OrderBy(u => u.Email),
                ("email", "desc") => query.OrderByDescending(u => u.Email),
                ("createdat", "asc") => query.OrderBy(u => u.CreatedAt),
                ("createdat", "desc") => query.OrderByDescending(u => u.CreatedAt),
                _ when sortDirection.ToLower() == "desc" => query.OrderByDescending(u => u.Id),
                _ => query.OrderBy(u => u.Id),
            };

            return await query.Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var existingUser = await context.Users.FindAsync(user.Id);
            if (existingUser == null)
                return false;

            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Password = user.Password;
            existingUser.Salt = user.Salt;
            existingUser.TeamId = user.TeamId;
            existingUser.Role = user.Role;

            context.Users.Update(existingUser);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VerifyUserAsync(VerifyUserModel verifyUser)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == verifyUser.Email);
            if (user == null)
                return false;

            user.PhoneNumber = verifyUser.PhoneNumber;
            user.Role = verifyUser.Role;
            user.TeamId = verifyUser.TeamId;

            context.Users.Update(user);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectUserAsync(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.Role = Roles.None;
            user.TeamId = null;

            context.Users.Update(user);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
                return false;

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // log error if needed
                return false;
            }
        }

    }
}
