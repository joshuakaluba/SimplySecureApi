using SimplySecureApi.Data.Models.Domain.Entity;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.Panics
{
    public interface IPanicRepository
    {
        Task CreatePanic(Panic panic);
    }
}