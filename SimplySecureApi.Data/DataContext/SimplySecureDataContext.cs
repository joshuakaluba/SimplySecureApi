using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Notification;
using SimplySecureApi.Data.Models.Static;

namespace SimplySecureApi.Data.DataContext
{
    public class SimplySecureDataContext : IdentityDbContext<ApplicationUser>
    {
        internal DbSet<ModuleEvent> ModuleEvents { get; set; }

        internal DbSet<Location> Locations { get; set; }

        internal DbSet<Module> Modules { get; set; }

        internal DbSet<Panic> Panics { get; set; }

        internal DbSet<LocationUser> LocationUsers { get; set; }

        internal DbSet<LocationActionEvent> LocationActionEvents { get; set; }

        internal DbSet<BootMessage> BootMessages { get; set; }

        internal DbSet<PushNotificationToken> PushNotificationTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString
                = $"Server={ApplicationConfig.DatabaseHost};" +
                    $"database={ApplicationConfig.DatabaseName};" +
                        $"uid={ApplicationConfig.DatabaseUser};" +
                            $"pwd={ApplicationConfig.DatabasePassword};" +
                                "pooling=true;";

            optionsBuilder.UseMySql(connectionString);
        }
    }
}