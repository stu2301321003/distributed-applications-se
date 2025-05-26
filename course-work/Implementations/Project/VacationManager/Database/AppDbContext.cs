using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VacationManager.Companies.Entities;
using VacationManager.Leaves.Entities;
using VacationManager.Teams.Entities;
using VacationManager.Users.Entities;

namespace VacationManager.Database
{
    public class AppDbContext(DataBaseSettings connectionStrings) : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (connectionStrings.UseInMemory)
            {
                optionsBuilder.UseInMemoryDatabase("VacationManager");
            }
            else
            {
                optionsBuilder.UseSqlServer(connectionStrings.ConnectionString);
            }

            //optionsBuilder.UseSqlServer("Server=DESKTOP-DR145US\\SQLEXPRESS;Database=MVR2;Trusted_Connection=True;");

            //   optionsBuilder.UseSqlServer("Server=DESKTOP-DR145US\\SQLEXPRESS;Database=MVR2;Trusted_Connection=True;");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Leave>().Property(l => l.Type).HasConversion<string>();
            modelBuilder.Entity<User>().Property(l => l.Role).HasConversion<string>();

            // Each Team has one Manager (who is a User), and each User may manage zero or one Teams
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Manager)
                .WithMany() // No navigation back from User to managed team
                .HasForeignKey(t => t.ManagerId)
                .OnDelete(DeleteBehavior.Restrict); // Optional, to prevent cascade delete

            // Each User can optionally be assigned to a Team (membership)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Team)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TeamId);
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Leave> Leaves { get; set; }
    }
}
