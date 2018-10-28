using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SimplySecureApi.Data.Models
{
    public class ViewResponseAlert
    {
        public ViewResponseAlert()
        {
        }

        public ViewResponseAlert(ViewResponseStatus status, string message)
        {
            Status = status;

            Message = message;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public ViewResponseStatus Status { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return Status + "||" + Message;
        }

        public static string GetStringResponse(ViewResponseStatus status, string message)
        {
            var customResponseAlert = new ViewResponseAlert(status, message);

            return customResponseAlert.ToString();
        }

        public static string ToJson(ViewResponseStatus status, string message)
        {
            var customResponseAlert = new ViewResponseAlert(status, message);

            return JsonConvert.SerializeObject(customResponseAlert);
        }
    }
}