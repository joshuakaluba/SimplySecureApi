using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Notification;
using SimplySecureApi.Data.Models.Static;
using System;

namespace SimplySecureApi.Data.DataContext
{
    public class SimplySecureDataContext : IdentityDbContext<ApplicationUser>
    {
        internal DbSet<ModuleEvent> ModuleEvents { get; set; }

        public DbSet<Location> Locations { get; set; }

        internal DbSet<Module> Modules { get; set; }

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

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<LocationActionEvent>()
                .Property(e => e.Action)
                .HasConversion(
                    v => v.ToString(),
                    v => (LocationActionEnum)Enum.Parse(typeof(LocationActionEnum), v));
        }*/
    }
}