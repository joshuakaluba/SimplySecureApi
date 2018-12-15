using SimplySecureApi.Data.Models.Domain.Entity;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.Services.Messaging
{
    public interface IMessagingService
    {
        Task SendModuleTriggeredMessage(Module module);

        Task SendModuleOfflineMessage(Module module);
    }
}