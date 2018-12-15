using SimplySecureApi.Common.Messaging;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Static;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.Services.Messaging
{
    public class MessagingService : IMessagingService
    {
        public async Task SendModuleTriggeredMessage(Module module)
        {
            var sid = ApplicationConfig.TwilioAccountSId;
            var authToken = ApplicationConfig.TwilioAuthenticationToken;
            var sender = ApplicationConfig.TwilioSenderPhoneNumber;
            var recipient = ApplicationConfig.TwilioRecipientPhoneNumber;

            var content = @"An alarm has been triggered at location: " + $"{module.Location.Name}, for module: {module.Name}.";

            var twilioMessage = new TwilioMessage(sid, authToken, sender);

            await twilioMessage.Send(recipient, content);
        }

        public async Task SendModuleOfflineMessage(Module module)
        {
            var sid = ApplicationConfig.TwilioAccountSId;
            var authToken = ApplicationConfig.TwilioAuthenticationToken;
            var sender = ApplicationConfig.TwilioSenderPhoneNumber;
            var recipient = ApplicationConfig.TwilioRecipientPhoneNumber;

            var content = $"Module: {module.Name} at location: " + $"{module.Location.Name} has changed to 'Offline'";

            var twilioMessage = new TwilioMessage(sid, authToken, sender);

            await twilioMessage.Send(recipient, content);
        }
    }
}