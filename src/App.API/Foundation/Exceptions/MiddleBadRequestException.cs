namespace App.API.Foundation.Exceptions
{
    using System;

    public class MiddleBadRequestException : Exception
    {
        public MiddleBadRequestException() : base()
        {
        }

        public MiddleBadRequestException(string message) : base(message)
        {
        }

        public MiddleBadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        // Aquí puedes añadir cualquier propiedad adicional que pueda ser relevante para la excepción.
    }

}
