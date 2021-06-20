using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Payroll.Models;

namespace Payroll.DataAccess
{
    public class PayrollDB : DbContext
    {
        private readonly IConfiguration configuration;
        private readonly ILoggerFactory logger;
        public PayrollDB(IConfiguration _configuration, ILoggerFactory _logger)
        {
            configuration = _configuration;
            logger = _logger;
        }

        public DbSet<Bank> Bank { set; get; }
        public DbSet<Customer> Customer { set; get; }
        public DbSet<District> District { set; get; }
        public DbSet<Employee> Employee { set; get; }
        public DbSet<EmploymentStatus> EmploymentStatus { set; get; }
        public DbSet<FamilyStatus> FamilyStatus { set; get; }
        public DbSet<Location> Location { set; get; }
        public DbSet<Position> Position { set; get; }
        public DbSet<Role> Role { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(this.logger);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseMySQL(configuration.GetConnectionString("PayrollDB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Bank>(bank =>
            {
                bank.HasKey(col => col.Code);
            });

            modelBuilder.Entity<Customer>(customer =>
            {
                customer.HasKey(col => col.Id);
            });

            modelBuilder.Entity<District>(district =>
            {
                district.HasKey(col => col.Id);
            });

            modelBuilder.Entity<Employee>(employee =>
            {
                employee.HasKey(col => col.NIK);        
            });

            modelBuilder.Entity<EmploymentStatus>(employmentStatus =>
            {
                employmentStatus.HasKey(col => col.Id);
            });

            modelBuilder.Entity<FamilyStatus>(familyStatus =>
            {
                familyStatus.HasKey(col => col.Code);
            });

            modelBuilder.Entity<Location>(location =>
            {
                location.HasKey(col => col.Id);
            });

            modelBuilder.Entity<Position>(position =>
            {
                position.HasKey(col => col.Id);
            });

            modelBuilder.Entity<Role>(role =>
            {
                role.HasKey(col => col.Id);
            });

        }
    }

}
