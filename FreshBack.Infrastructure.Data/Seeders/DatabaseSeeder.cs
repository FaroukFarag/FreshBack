using FreshBack.Domain.Enums.Roles;
using FreshBack.Domain.Interfaces.Seeders;
using FreshBack.Domain.Models.Roles;
using FreshBack.Domain.Models.Settings.Users;
using FreshBack.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FreshBack.Infrastructure.Data.Seeders;

public class DatabaseSeeder(
    FreshBackDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    ILogger<DatabaseSeeder> logger,
    IServiceProvider serviceProvider) : IDatabaseSeeder
{
    private readonly FreshBackDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly ILogger<DatabaseSeeder> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task SeedAsync()
    {
        try
        {
            if (!await _context.Roles.AnyAsync())
            {
                _logger.LogInformation("Seeding default roles...");

                await SeedRolesAsync();
            }

            if (!await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Seeding default users...");

                await SeedUsersAsync();
            }

            _logger.LogInformation("Database seeding completed successfully.");
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task SeedRolesAsync()
    {
        foreach (RoleNames roleName in Enum.GetValues(typeof(RoleNames)))
        {
            var roleNameString = roleName.ToString();

            if (!await _roleManager.RoleExistsAsync(roleNameString))
            {
                var role = new Role
                {
                    Id = (int)roleName,
                    Name = roleNameString
                };

                await _roleManager.CreateAsync(role);

                _logger.LogInformation("Created role: {RoleName} with ID: {RoleId}", roleNameString, role.Id);
            }
        }
    }

    private async Task SeedUsersAsync()
    {
        var adminUser = new User
        {
            UserName = "Admin",
            Email = "admin@admin.com",
            PhoneNumber = "+1234567890",
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(adminUser, "Admin@12345");

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(adminUser, RoleNames.Admin.ToString());

            _logger.LogInformation("Admin user created successfully.");
        }

        else
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("User creation error: {Error}", error.Description);
            }
        }
    }
}

