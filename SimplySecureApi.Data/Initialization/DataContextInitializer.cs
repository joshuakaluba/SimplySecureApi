using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Domain.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.Initialization
{
    public static class DataContextInitializer
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var db = new SimplySecureDataContext())
                {
                    if (db.Database.GetPendingMigrations().Any())
                    {
                        await db.Database.MigrateAsync();

                        var defaultModule = await db.Modules.FirstOrDefaultAsync();

                        if (defaultModule == null)
                        {
                            var module = new Module();

                            db.Modules.Add(module);

                            await db.SaveChangesAsync();
                        }
                    }
                }
            }
        }
    }
}