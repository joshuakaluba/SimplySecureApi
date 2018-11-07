using System;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.Modules
{
    public interface IModuleRepository
    {
        Task<Models.Domain.Entity.Module> FindModule(Guid moduleId);

        Task TriggerModule(Models.Domain.Entity.Module module);
    }
}