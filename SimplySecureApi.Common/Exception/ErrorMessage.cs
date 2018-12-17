using System;

namespace SimplySecureApi.Common.Exception
{
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            this.Message = message;
        }

        public ErrorMessage(System.Exception exception)
        {
            this.Message = exception.Message;
        }

        public String Message { get; set; }
    }
}