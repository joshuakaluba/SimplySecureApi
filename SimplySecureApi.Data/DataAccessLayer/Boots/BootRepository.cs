using System.Threading.Tasks;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Domain.Entity;

namespace SimplySecureApi.Data.DataAccessLayer.Boots
{
    public class BootRepository : BaseRepository, IBootRepository
    {
        public async Task SaveBootMessage(BootMessage bootMessage)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.BootMessages.Add(bootMessage);

                await DataContext.SaveChangesAsync();
            }
        }
    }
}
