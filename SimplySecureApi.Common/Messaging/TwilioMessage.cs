using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SimplySecureApi.Common.Messaging
{
    public class TwilioMessage
    {
        private readonly string _accountSid;
        private readonly string _authenticationToken;
        private readonly string _senderPhoneNumber;

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

            await MessageResource.CreateAsync(messageOptions);
        }
    }
}