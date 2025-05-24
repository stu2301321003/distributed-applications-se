using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace VacationManager.Database
{
    public class AppDbContext(IOptions<ConnectionStrings> connectionStrings) : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=DESKTOP-DR145US\\SQLEXPRESS;Database=MVR2;Trusted_Connection=True;");

            //   optionsBuilder.UseSqlServer("Server=DESKTOP-DR145US\\SQLEXPRESS;Database=MVR2;Trusted_Connection=True;");
            optionsBuilder.UseSqlServer(connectionStrings.Value.Database);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Leave>().Property(l => l.Type).HasConversion<string>();
            modelBuilder.Entity<User>().Property(l => l.Role).HasConversion<string>();
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Leave> Leaves { get; set; }
    }
}
