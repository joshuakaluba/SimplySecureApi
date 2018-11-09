using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Domain.Entity;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.StateChanges
{
    public class StateChangesRepository : BaseRepository, IStateChangesRepository
    {
        public async Task SaveStateChange(ModuleStateChange stateChange)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.ModuleStateChanges.Add(stateChange);

                await DataContext.SaveChangesAsync();
            }
        }
    }
}