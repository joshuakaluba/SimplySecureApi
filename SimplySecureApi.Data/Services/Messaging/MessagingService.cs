using Floxdc.ExponentServerSdk;
using SimplySecureApi.Common.Messaging;
using SimplySecureApi.Data.DataAccessLayer.LocationUsers;
using SimplySecureApi.Data.DataAccessLayer.PushNotificationTokens;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Static;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimplySecureApi.Data.Models.Notification;
using Module = SimplySecureApi.Data.Models.Domain.Entity.Module;

namespace SimplySecureApi.Data.Services.Messaging
{
    public class MessagingService : IMessagingService
    {
        private readonly ILocationUsersRepository _locationUsersRepository;
        private readonly IPushNotificationTokensRepository _pushNotificationTokensRepository;

        public MessagingService()
        {
            _locationUsersRepository = new LocationUsersRepository();

            _pushNotificationTokensRepository = new PushNotificationTokensRepository();
        }

        private async Task SendPushNotification(Location location, string title, string content)
        {
            var applicationUsers = await _locationUsersRepository.GetLocationApplicationUsers(location);

            var pushNotificationTokens =
                await _pushNotificationTokensRepository.GetLocationsPushNotificationTokens(applicationUsers);

            foreach (var token in pushNotificationTokens)
            {
                await SendExpoMessage(token.Token, title, content);
            }
        }

        private async Task SendExpoMessage(string token, string title, string message)
        {
            try
            {
                var client = new PushClient();

                var notification = new PushMessage(token, title: title, body: message);

                await client.Publish(notification);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task SendTwilioMessage(string content)
        {
            var sid = ApplicationConfig.TwilioAccountSId;

            var authToken = ApplicationConfig.TwilioAuthenticationToken;

            var sender = ApplicationConfig.TwilioSenderPhoneNumber;

            var recipient = ApplicationConfig.TwilioRecipientPhoneNumber;

            var twilioMessage = new TwilioMessage(sid, authToken, sender);

            if (ApplicationConfig.SendTwillioMessage)
            {
                await twilioMessage.Send(recipient, content);
            }
        }

        public async Task SendModuleTriggeredMessage(Module module)
        {
            try
            {
                var content = @"An alarm has been triggered at location: " + $"{module.Location.Name}, for module: {module.Name}.";

                await SendTwilioMessage(content);

                await SendPushNotification(module.Location, "Alarm Triggered", content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task SendPanicAlarm(Location location, ApplicationUser user)
        {
            try
            {
                var applicationUsers = await _locationUsersRepository.GetLocationApplicationUsers(location);

                var pushNotificationTokens =
                    await _pushNotificationTokensRepository.GetLocationsPushNotificationTokens(applicationUsers);

                var content = $"Panic alarm triggered by {user.FullName}";

                var tokens = new HashSet<PushNotificationToken>();

                foreach (var token in pushNotificationTokens)
                {
                    if (tokens.Add(token))
                    {
                        await SendExpoMessage(token.Token, "Panic Alarm Triggered", content);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task SendModuleOfflineMessage(Module module)
        {
            try
            {
                var content = $"Module: {module.Name} at location: " + $"{module.Location.Name} has changed to 'Offline'";

                await SendTwilioMessage(content);

                await SendPushNotification(module.Location, "Module Office", content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}