using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Payroll.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Payroll.DataAccess
{
    public class PayrollDB : DbContext
    {
        private readonly IConfiguration configuration;
        private readonly ILoggerFactory logger;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PayrollDB(IConfiguration _configuration, ILoggerFactory _logger, IHttpContextAccessor _httpContextAccessor)
        {
            configuration = _configuration;
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            {
                if (httpContextAccessor != null && httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.User != null && httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    var email = httpContextAccessor.HttpContext.User.GetEmail();
                    Employee employee = Employee.Where(column => column.Email == email).FirstOrDefault();
                    if (employee != null)
                    {
                        var entries = ChangeTracker.Entries().ToList();
                        foreach (var entry in entries)
                        {
                            if (entry.Entity is Models.Audit audit)
                            {
                                switch (entry.State)
                                {
                                    case EntityState.Added:
                                        audit.CreateBy = employee.NIK;
                                        audit.CreateDateUtc = DateTime.UtcNow;
                                        audit.ModifyBy = employee.NIK;
                                        audit.ModifyDateUtc = DateTime.UtcNow;
                                        break;
                                    case EntityState.Modified:
                                        audit.ModifyBy = employee.NIK;
                                        audit.ModifyDateUtc = DateTime.UtcNow;
                                        Entry(audit).Property(p => p.CreateBy).IsModified = false;
                                        Entry(audit).Property(p => p.CreateDateUtc).IsModified = false;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}
