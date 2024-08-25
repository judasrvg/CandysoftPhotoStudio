namespace App.API.Foundation.Exceptions
{
    public class MiddleUnauthorizedException : Exception
    {
        public MiddleUnauthorizedException() : base()
        {
        }

        public MiddleUnauthorizedException(string message) : base(message)
        {
        }

        public MiddleUnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
