using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Static;

namespace SimplySecureApi.Data.DataContext
{
    public class SimplySecureDataContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Module> Modules { get; set; }

        internal DbSet<BootMessage> BootMessages { get; set; }

        internal DbSet<TriggeredModule> TriggeredModules { get; set; }

        internal DbSet<ModuleStateChange> ModuleStateChanges { get; set; }

        internal DbSet<ArmedModule> ArmedModules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString
                = $"Server={ApplicationConfig.DatabaseHost};" +
                    $"database={ApplicationConfig.DatabaseName};" +
                        $"uid={ApplicationConfig.DatabaseUser};" +
                            $"pwd={ApplicationConfig.DatabasePassword};" +
                                $"pooling=true;";

            optionsBuilder.UseMySql(connectionString);
        }
    }
}