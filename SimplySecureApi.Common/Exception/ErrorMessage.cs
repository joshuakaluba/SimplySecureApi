namespace SimplySecureApi.Common.Exception
{
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            Message = message;
        }

        public ErrorMessage(System.Exception exception)
        {
            Message = exception.Message;
        }

        public string Message { get; set; }
    }
}