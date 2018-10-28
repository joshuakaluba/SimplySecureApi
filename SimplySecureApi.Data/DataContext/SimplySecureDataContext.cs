using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Static;

namespace SimplySecureApi.Data.DataContext
{
    public class SimplySecureDataContext : IdentityDbContext<ApplicationUser>
    {
        public SimplySecureDataContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString
                = $"Server={ApplicationKeys.DatabaseHost};" +
                    $"database={ApplicationKeys.DatabaseName};" +
                        $"uid={ApplicationKeys.DatabaseUser};" +
                            $"pwd={ApplicationKeys.DatabasePassword};" +
                                $"pooling=true;";

            optionsBuilder.UseMySql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}