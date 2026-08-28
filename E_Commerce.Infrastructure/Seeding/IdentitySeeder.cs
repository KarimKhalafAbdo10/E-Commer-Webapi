using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Identity.Data;
using E_Commerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Seeding
{
    internal class IdentitySeeder : IDataSeeder
    {
        private readonly StoreIdentityDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<StoreIdentityDbContext> _logger;

        public IdentitySeeder(StoreIdentityDbContext dbContext, UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, ILogger<StoreIdentityDbContext> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
  var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(ct);

            if (pendingMigrations.Any())
             await   _dbContext.Database.MigrateAsync(ct);


            if(!await _roleManager.Roles.AnyAsync())
            {
               await _roleManager.CreateAsync(new IdentityRole("Admin"));
               await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            if(!await _userManager.Users.AnyAsync())
            {
                var Admin = new ApplicationUser()
                {
                    DisplayName = "Karim Khalaf",
                    Email = "Karim@gmail.com",
                    UserName = "karimKhalaf",
                    PhoneNumber = "01234567890",
                };

               var createReslut= await _userManager.CreateAsync(Admin, "P@ssW0rd");

                if (createReslut.Succeeded)
                {
                   await _userManager.AddToRoleAsync(Admin, "SuperAdmin");
                }
                else
                {
                    var errors = string.Join(";",createReslut.Errors.Select(e => e.Description));
                    _logger.LogWarning($"can't Seed Dafault Admin{errors}");
                }

            }
            }

            catch (Exception ex) 
            {


                _logger.LogError(ex, "Identity Data Seeding Faild");
                return;
                    }
            

        }
    }
}
