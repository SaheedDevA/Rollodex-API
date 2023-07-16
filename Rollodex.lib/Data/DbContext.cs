using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Rollodex.lib.Models.Entities;
using Rolodex.Lib.Utils.Helpers;
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

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor context,
            IConfiguration configuration
        )
            : base(options)
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
            // Check if any records exist in the tables
            if (_dbContext.Categories.Any())
            {
                return; // Data already seeded, no need to continue
            }

            // Seed super admin account
            _dbContext.Accounts.AddRange(
                new Account
                {
                    Created = DateTime.UtcNow,
                    Email = "dosamuyimnen@gmail.com",
                    FirstName = "Dennis",
                    IsDisabled = false,
                    LastName = "Osagiede",
                    Username = "Dennis247",
                    LastTimeLoggedIn = DateTime.UtcNow,
                    LoggedOutTime = DateTime.UtcNow,
                    PasswordHash = SecureTextHasher.Hash("Nappyboy@247"),
                    VerificationToken = "",
                    PhoneNumber = "08167828256",
                    UserType = Constants.Admin,
                }
            );


            //seed categories
            _dbContext.Categories.AddRange(
                new Category
                {
                    CategoryName = "Expertise",
                    CategoryId = 1,
                    Created = DateTime.UtcNow,
                    CreatedBy = 1,
                    LastUpdatedBy = 1,
                },
                new Category
                {
                    CategoryName = "Skills",
                    CategoryId = 2,
                    Created = DateTime.UtcNow,
                    CreatedBy = 1,
                    LastUpdatedBy = 1,
                },
                new Category
                {
                    CategoryName = "Sector",
                    CategoryId = 3,
                    Created = DateTime.UtcNow,
                    CreatedBy = 1,
                    LastUpdatedBy = 1,
                },
                new Category
                {
                    CategoryName = "Geography",
                    Created = DateTime.UtcNow,
                    CreatedBy = 1,
                    LastUpdatedBy = 1,
                },
                new Category
                {
                    CategoryName = "Services",
                    CategoryId = 4,
                    Created = DateTime.UtcNow,
                    CreatedBy = 1,
                    LastUpdatedBy = 1,
                },
                 new Category
                 {
                     CategoryName = "Other Expertise",
                     CategoryId = 5,
                     Created = DateTime.UtcNow,
                     CreatedBy = 1,
                     LastUpdatedBy = 1,
                 }
            );

            _dbContext.SaveChanges();
        }
    }
}
