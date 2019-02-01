using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using SimplySecureApi.Data.Services;
using SimplySecureApi.Data.Services.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplySecureApi.Data.DataAccessLayer.LocationActionEvents;

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

        public async Task<List<Module>> GetAllModules()
        {
            using (DataContext = new SimplySecureDataContext())
            {
                var modules
                    = await DataContext.Modules.Include(m => m.Location).ToListAsync();

                return modules;
            }
        }

        public async Task<List<Module>> GetModulesByLocation(Location location)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                var modules
                    = await DataContext
                        .Modules
                            .Where(b => b.LocationId == location.Id)
                                .Include(m => m.Location).ToListAsync();
                return modules;
            }
        }

        public async Task UpdateModuleState(Module module, bool state)
        {
            module.State = state;

            await UpdateModule(module);
        }

        public async Task UpdateModuleLastBoot(Module module)
        {
            module.LastBoot = DateTime.UtcNow;

            await UpdateModule(module);
        }

        public async Task UpdateModule(Module module)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.Modules.Update(module);

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task DeleteModule(Module module)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.Modules.Remove(module);

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task CreateModule(Module module)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.Modules.Add(module);

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task UpdateModuleHeartbeats(
            List<ModuleViewModel> modulesViewModels, 
            ILocationRepository locationRepository, 
            IMessagingService messagingService, 
            ILocationActionEventsRepository locationActionEventsRepository)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                var modules
                    = await DataContext.Modules.Where
                        (module => modulesViewModels.Any(vm => vm.Id == module.Id))
                            .Include(m => m.Location)
                                .ToListAsync();

                foreach (var module in modules)
                {
                    var moduleViewModel
                        = modulesViewModels.Single
                            (m => m.Id == module.Id);

                    if (moduleViewModel.LastHeartbeat >= module.LastHeartbeat.AddSeconds(10))
                    {
                        module.LastHeartbeat = moduleViewModel.LastHeartbeat;
                        module.Offline = false;
                    }

                    DataContext.Modules.Update(module);

                    if (module.Location.Armed && module.State != moduleViewModel.State)
                    {
                        //this should never happen but it is here for completeness
                        await LocationTriggeringService.DetermineIfTriggering(locationRepository, locationActionEventsRepository ,messagingService, module);
                    }
                }

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task ProcessOfflineModules(
            ILocationRepository locationRepository, 
            IMessagingService messagingService, 
            ILocationActionEventsRepository locationActionEventsRepository)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                var modules
                    = await DataContext.Modules
                        .Where(m => m.Offline == false && (m.LastHeartbeat < DateTime.Now.AddMinutes(-5)))
                            .Include(m => m.Location)
                                .ToListAsync();

                foreach (var module in modules)
                {
                    module.Offline = true;

                    DataContext.Modules.Update(module);

                    await DataContext.SaveChangesAsync();

                    if (module.Location.Armed)
                    {
                        await LocationTriggeringService.DetermineIfTriggering(locationRepository,locationActionEventsRepository, messagingService, module);
                    }
                    else
                    {
                        await messagingService.SendModuleOfflineMessage(module);
                    }
                }
            }
        }
    }
}