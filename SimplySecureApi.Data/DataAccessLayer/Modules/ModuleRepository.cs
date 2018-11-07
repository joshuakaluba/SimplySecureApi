﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using SimplySecureApi.Data.Models.Domain.Entity;

namespace SimplySecureApi.Data.DataAccessLayer.Modules
{
    public class ModuleRepository : BaseRepository, IModuleRepository
    {
        public async Task<Module> FindModule(Guid moduleId)
        {
            var module
                = await DataContext.Modules
                    .Where(m => m.Id == moduleId)
                        .FirstOrDefaultAsync();

            return module;
        }

        public async Task TriggerModule(Module module)
        {
            module.Triggered = true;

            DataContext.Modules.Update(module);

            await DataContext.SaveChangesAsync();
        }
    }
}