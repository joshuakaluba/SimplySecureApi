using SimplySecureApi.Data.Models.Domain.Entity;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.StateChanges
{
    public interface IStateChangesRepository
    {
        Task SaveStateChange(ModuleStateChange stateChange);
    }
}