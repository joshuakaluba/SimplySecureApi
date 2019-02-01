using SimplySecureApi.Data.Models.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.LocationActionEvents
{
    public interface ILocationActionEventsRepository
    {
        Task SaveLocationActionEvent(LocationActionEvent locationActionEvent);

        Task<List<LocationActionEvent>> GetLocationActionEventsByLocation(Location location);
    }
}