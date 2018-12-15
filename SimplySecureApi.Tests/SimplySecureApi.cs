using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimplySecureApi.Common.Messaging;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Static;
using SimplySecureApi.Data.Services.Messaging;

namespace SimplySecureApi.Tests
{
    [TestClass]
    public class SimplySecureApi
    {
        [TestMethod]
        [ExpectedException(typeof(System.AggregateException))]
        public void ModuleInValidLocation()
        {
            var module = new Module { Location = null };

            var messagingService = new MessagingService();

            messagingService.SendModuleTriggeredMessage(module).Wait();
        }

        [TestMethod]
        public void ModuleValidLocation()
        {
            var location = new Location
            {
                Name = "Test Location",
                Armed = true,
                IsSilentAlarm = false,
                Triggered = false
            };

            var module = new Module
            {
                Name = "Main Door",
                Location = location,
                State = false
            };

            var messagingService = new MessagingService();

            messagingService.SendModuleTriggeredMessage(module).Wait();
        }

        [TestMethod]
        public void SendTwilioMessage()
        {
            var twilioMessage
                = new TwilioMessage(ApplicationConfig.TwilioAccountSId,
                ApplicationConfig.TwilioAuthenticationToken, ApplicationConfig.TwilioSenderPhoneNumber);

            twilioMessage.Send(ApplicationConfig.TwilioRecipientPhoneNumber, "Test Message from tests").Wait();
        }
    }
}