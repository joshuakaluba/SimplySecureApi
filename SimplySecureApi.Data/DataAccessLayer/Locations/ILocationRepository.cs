using SimplySecureApi.Data.Models.Domain.Entity;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.Locations
{
    public interface ILocationRepository
    {
        Task TriggerLocation(Location location);
    }
}