using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SimplySecureApi.Common.Messaging
{
    public class TwilioMessage
    {
        private string _accountSid = "";
        private string _authenticationToken = "";
        private string _senderPhoneNumber = "";

        public TwilioMessage(string accountSid, string authenticationToken, string senderPhoneNumber)
        {
            _accountSid = accountSid;

            _authenticationToken = authenticationToken;

            _senderPhoneNumber = senderPhoneNumber;
        }

        public async Task Send(string recipientPhoneNumber, string messageContent)
        {
            TwilioClient.Init(_accountSid, _authenticationToken);

            var messageOptions = new CreateMessageOptions(new PhoneNumber(recipientPhoneNumber));

            messageOptions.From = new PhoneNumber(_senderPhoneNumber);

            messageOptions.Body = messageContent;

            var message = await MessageResource.CreateAsync(messageOptions);
        }
    }
}