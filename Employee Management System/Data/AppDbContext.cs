
using Employee_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                        .HasIndex(e => e.Email)
                        .IsUnique();

            modelBuilder.Entity<Employee>()
                        .Property(e => e.Salary)
                        .HasColumnType("decimal(18,2)");

            // Optional seed data
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    FirstName = "Owele",
                    LastName = "Ngwenya",
                    Email = "owele@example.com",
                    Phone = "010-000-0000",
                    HireDate = new DateTime(2024, 1, 1),
                    Department = "IT",
                    Salary = 30000,
                    IsActive = true
                }
            );
        }
    }
}
