using System.Threading.Tasks;
using SimplySecureApi.Data.Models.Domain.Entity;

namespace SimplySecureApi.Data.DataAccessLayer.Boots
{
    public interface IBootRepository
    {
        Task SaveBootMessage(BootMessage bootMessage);
    }
}