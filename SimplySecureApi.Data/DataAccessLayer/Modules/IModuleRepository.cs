using SimplySecureApi.Data.Models.Domain.Entity;
using System;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.Modules
{
    public interface IModuleRepository
    {
        Task<Module> FindModule(Guid moduleId);

        Task UpdateModuleState(Module module, bool state);
    }
}