using SimplySecureApi.Data.DataAccessLayer.LocationActionEvents;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using SimplySecureApi.Data.Services.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.Modules
{
    public interface IModuleRepository
    {
        Task<Module> FindModule(Guid moduleId);

        Task<List<Module>> GetModulesByLocation(Location location);

        Task UpdateModuleState(Module module, bool state);

        Task UpdateModuleLastBoot(Module module);

        Task UpdateModule(Module module);

        Task DeleteModule(Module module);

        Task CreateModule(Module module);

        Task UpdateModuleHeartbeats(List<ModuleViewModel> modulesViewModels, ILocationRepository locationRepository, IMessagingService messagingService, ILocationActionEventsRepository locationActionEventsRepository);

        Task ProcessOfflineModules(ILocationRepository locationRepository, IMessagingService messagingService, ILocationActionEventsRepository locationActionEventsRepository);
    }
}