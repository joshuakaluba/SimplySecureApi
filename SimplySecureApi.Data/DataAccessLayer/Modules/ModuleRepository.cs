using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Domain.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.Modules
{
    public class ModuleRepository : BaseRepository, IModuleRepository
    {
        public async Task<Module> FindModule(Guid moduleId)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                var module
                    = await DataContext.Modules
                        .Where(m => m.Id == moduleId)
                            .Include(m => m.Location)
                                .FirstOrDefaultAsync();

                return module;
            }
        }

        public async Task UpdateModuleState(Module module, bool state)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                module.State = state;

                DataContext.Modules.Update(module);

                await DataContext.SaveChangesAsync();
            }
        }
    }
}