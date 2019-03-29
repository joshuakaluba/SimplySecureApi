using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Domain.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.ModuleEvents
{
    public class ModuleEventRepository : BaseRepository, IModuleEventRepository
    {
        public async Task SaveModuleEvent(ModuleEvent moduleEvent)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.ModuleEvents.Add(moduleEvent);

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task<List<ModuleEvent>> GetModuleEventsByLocation(Location location)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                var moduleEvents
                    = await DataContext.ModuleEvents
                        .Where(m => m.Module.LocationId == location.Id)
                            .Include(m => m.Module)
                                .OrderByDescending(m => m.DateCreated)
                                    .Take(200)
                                        .ToListAsync();

                return moduleEvents;
            }
        }
    }
}