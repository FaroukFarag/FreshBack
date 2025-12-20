using FreshBack.Domain.Models.Roles;
using FreshBack.Domain.Models.Users;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Roles;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreshBack.Infrastructure.Data.Context;

public class FreshBackDbContext(DbContextOptions options) : IdentityDbContext<User, Role, int>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfigurations());
        modelBuilder.ApplyConfiguration(new RoleConfigurations());
    }
}
