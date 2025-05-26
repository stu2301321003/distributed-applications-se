using Microsoft.EntityFrameworkCore;
using VacationManager.Auth.Helpers;
using VacationManager.Auth.Models;
using VacationManager.Users.Entities;

namespace VacationManager.Database
{
    public static class InMemoryDbSeeder
    {
        public static async Task Seed(AppDbContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Avoid duplicate seeding
            if (await context.Users.AnyAsync())
                return;

            string salt = PasswordHelper.GenerateSalt();
            string testPassword = "dev-pass";
            string hashedPassword = PasswordHelper.HashPassword(testPassword, salt);

            User newUser = new()
            {
                Name = "Developer",
                LastName = "Developer",
                Email = "dev@example.com",
                PhoneNumber = "+359877527763",
                Password = hashedPassword,
                Salt = salt,
                Role = Roles.Dev,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(newUser);
            await context.SaveChangesAsync();
        }
    }
}
