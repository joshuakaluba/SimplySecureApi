using Newtonsoft.Json;
using System;

namespace SimplySecureApi.Data.Models.Response
{
    public class ErrorResponse
    {
        public ErrorResponse(string message)
        {
            Message = message;
        }

        public ErrorResponse(Exception exception)
        {
            Message = exception.Message;
        }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}