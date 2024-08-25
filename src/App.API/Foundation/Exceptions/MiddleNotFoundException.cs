namespace App.API.Foundation.Exceptions
{
    public class MiddleNotFoundException : Exception
    {
        public MiddleNotFoundException() : base()
        {
        }

        public MiddleNotFoundException(string message) : base(message)
        {
        }

        public MiddleNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
