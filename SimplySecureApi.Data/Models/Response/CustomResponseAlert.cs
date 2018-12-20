using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;

namespace SimplySecureApi.Data.Models.Response
{
    public enum ResponseStatusEnum
    {
        Success,
        Danger
    }

    public class CustomResponseAlert
    {
        public CustomResponseAlert()
        {
        }

        public CustomResponseAlert(ResponseStatusEnum status, string message)
        {
            this.Status = status;

            this.Message = message;
        }

        public override string ToString()
        {
            return Status.ToString() + "||" + Message;
        }

        [JsonConverter(typeof(StringEnumConverter<,,>))]
        public ResponseStatusEnum Status { get; set; }

        public string Message { get; set; }

        public static string GetStringResponse(ResponseStatusEnum status, string message)
        {
            var customResponseAlert = new CustomResponseAlert(status, message);

            return customResponseAlert.ToString();
        }

        public static string ToJson(ResponseStatusEnum status, string message)
        {
            var customResponseAlert = new CustomResponseAlert(status, message);

            return JsonConvert.SerializeObject(customResponseAlert);
        }
    }
}