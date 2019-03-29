using System;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Domain.Entity;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.Panics
{
    public class PanicRepository : BaseRepository, IPanicRepository
    {
        public async Task CreatePanic(Panic panic)
        {
            panic.Id = Guid.NewGuid();

            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.Panics.Add(panic);

                await DataContext.SaveChangesAsync();
            }
        }
    }
}