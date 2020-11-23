using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Identity

            builder.Entity<User>(e => e.ToTable("Users"));
            builder.Entity<IdentityRole>(e => e.ToTable("Roles"));
            builder.Entity<IdentityUserRole<string>>(e =>
            {
                e.ToTable("UserRoles");
                // In case you changed the TKey type
                //e.HasKey(key => new { key.UserId, key.RoleId });
            });
            builder.Entity<IdentityUserClaim<string>>(e => e.ToTable("UserClaims"));
            builder.Entity<IdentityUserLogin<string>>(e =>
            {
                e.ToTable("UserLogins");
                // In case you changed the TKey type
                //e.HasKey(key => new { key.ProviderKey, key.LoginProvider });
            });
            builder.Entity<IdentityRoleClaim<string>>(e => e.ToTable("RoleClaims"));
            builder.Entity<IdentityUserToken<string>>(e =>
            {
                e.ToTable("UserTokens");
                // In case you changed the TKey type
                //e.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
            });

            #endregion
        }
    }
}
