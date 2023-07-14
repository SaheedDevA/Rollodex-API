
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Rollodex.lib.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rolodex.Lib.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _context;
        public IDbConnection connection;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor context, IConfiguration configuration) : base(options)
        {
            _context = context;
            connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryItem> CategoryItems { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyContactInformation> CompanyContactInformations { get; set; }
        public DbSet<CompanyCategoryItem> CompanyCategoryItems { get; set; }
        public DbSet<EngagementHistory> EngagementHistories { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<PendingCategoryItem> PendingCategoryItems { get; set; }
        public DbSet<RolodexSystem> Systems { get; set; }
        public DbSet<UseCase> UseCases { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public void SeedData()
        {
            // Create the DbSeeder instance
            var dbSeeder = new DbSeeder(this);

            // Call the SeedData method
            dbSeeder.SeedData();
        }
    }

    // Define the DbSeeder class for seeding data
    public class DbSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public DbSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedData()
        {
        /*    // Check if any records exist in the tables
            if (_dbContext.Accounts.Any() || _dbContext.Teams.Any() || _dbContext.TeamMembers.Any() || _dbContext.Permissions.Any()
                || _dbContext.Modules.Any() || _dbContext.TeamModulePermissions.Any())
            {
                return; // Data already seeded, no need to continue
            }

            // Seed initial data for each table
            _dbContext.Accounts.AddRange(
                new Account
                {
                    Created = DateTime.UtcNow,
                    Email = "admin@intel.com",
                    FirstName = "admin",
                    IsDisabled = false,
                    LastName = "intel",
                    Username = "intel",
                    LastTimeLoggedIn = DateTime.UtcNow,
                    LoggedOutTime = DateTime.UtcNow,
                    PasswordHash = SecureTextHasher.Hash("Nappyboy@247"),
                    VerificationToken = "",
                    HasSetUpAuthenticator = true,
                    PhoneNumber = "08033344478",
                    UserType = Constants.SuperAdmin,
                }
            );

            _dbContext.Modules.AddRange(
            new Module { Name = "Team Manager" },
            new Module { Name = "Generate Report" },
            new Module { Name = "Clients" },
            new Module { Name = "API Integrations" },
            new Module { Name = "Logs" }
        );

            List<ApiCall> apiCalls = new List<ApiCall>();
            for (int i = 0; i < 5; i++)
            {
                ApiCall apiCall = new ApiCall()
                {
                    ApiUrl = $"https://api.example.com/{i}",
                    CLientId = i + 1,
                    IsSucessfull = i % 2 == 0,  // Alternate between true and false
                    Created = DateTime.Now.AddDays(i),
                    CreatedBy = i + 10,
                    LastUpdatedBy = i + 20
                };

                apiCalls.Add(apiCall);
            }

            _dbContext.ApiCalls.AddRange(apiCalls);

            _dbContext.TeamMembers.AddRange(
            // Add team member entities here
            );

            _dbContext.Permissions.AddRange(
            // Add permission entities here
            );

            _dbContext.Modules.AddRange(
            // Add module entities here
            );

            _dbContext.TeamModulePermissions.AddRange(
            // Add team module permission entities here
            );

            _dbContext.SaveChanges();*/
        }
    }
}
