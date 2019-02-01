using SimplySecureApi.Data.Models.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.ModuleEvents
{
    public interface IModuleEventRepository
    {
        Task SaveModuleEvent(ModuleEvent moduleEvent);

        Task<List<ModuleEvent>> GetModuleEvents();

        Task<List<ModuleEvent>> GetModuleEventsByLocation( Location location);
    }
}