using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Domain.Entity;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.Locations
{
    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public async Task TriggerLocation(Location location)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                location.Triggered = true;

                DataContext.Locations.Update(location);

                await DataContext.SaveChangesAsync();
            }
        }
    }
}