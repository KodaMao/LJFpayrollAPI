using LJFpayrollAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LJFpayrollAPI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.EmployeeNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(e => e.EmployeeNumber)
                    .IsUnique();

                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MiddleName).HasMaxLength(100);

                entity.Property(e => e.DateOfBirth)
                    .IsRequired()
                    .HasColumnType("date");

                entity.Property(e => e.DailyRate)
                    .IsRequired()
                    .HasPrecision(18, 2);


                entity.Property(e => e.WorkingDays)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });
        }
    }
}
