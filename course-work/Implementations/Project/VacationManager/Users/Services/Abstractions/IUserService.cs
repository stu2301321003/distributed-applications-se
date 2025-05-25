using VacationManager.Users.Entities;
using VacationManager.Users.Models;

namespace VacationManager.Users.Services.Abstractions
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByPhoneAsync(string phone);
        Task<IList<User>> GetUsersAsync(int page, int pageSize, string sortBy, string sortDirection);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> VerifyUserAsync(VerifyUserModel verifyUser);
        Task<bool> RejectUserAsync(int userId);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> AddUserAsync(User user);

    }

}
