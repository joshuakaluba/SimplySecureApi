using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Static;

namespace SimplySecureApi.Data.DataContext
{
    public class SimplySecureDataContext : IdentityDbContext<ApplicationUser>
    {
        public SimplySecureDataContext()
        {
        }

        internal DbSet<Module> Modules { get; set; }

        internal DbSet<BootMessage> BootMessages { get; set; }

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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}