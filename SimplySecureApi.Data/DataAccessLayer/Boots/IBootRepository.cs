using SimplySecureApi.Data.Models.Domain.Entity;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.Boots
{
    public interface IBootRepository
    {
        Task SaveBootMessage(BootMessage bootMessage);
    }
}