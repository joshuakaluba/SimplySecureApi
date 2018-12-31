﻿using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Domain.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.ModuleEvents
{
    public class ModuleEventRepository : BaseRepository, IModuleEventRepository
    {
        public async Task SaveModuleEvent(ModuleEvent moduleEvent)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.ModuleEvents.Add(moduleEvent);

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task<List<ModuleEvent>> GetModuleEvents()
        {
            using (DataContext = new SimplySecureDataContext())
            {
                var modules
                    = await DataContext.ModuleEvents
                        .Include(m => m.Module)
                            .OrderByDescending(m => m.DateCreated)
                                .ToListAsync();

                return modules;
            }
        }
    }
}