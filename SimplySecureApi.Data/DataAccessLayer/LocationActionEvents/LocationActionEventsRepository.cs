using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Domain.Entity;

namespace SimplySecureApi.Data.DataAccessLayer.LocationActionEvents
{
    public class LocationActionEventsRepository : BaseRepository, ILocationActionEventsRepository
    {
        public async Task SaveLocationActionEvent(LocationActionEvent locationActionEvent)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.LocationActionEvents.Add(locationActionEvent);

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task<List<LocationActionEvent>> GetLocationActionEventsByLocation(Location location)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                var actionEvents 
                    = await DataContext.LocationActionEvents
                        .Where(a => a.LocationId == location.Id)
                            .Include(e=>e.ApplicationUser)
                                .OrderByDescending(l => l.DateCreated)
                                    .ToListAsync();

                return actionEvents;
            }
        }
    }
}
