
namespace Tattoo.Management.Models.Requests
{
    
    public class ResponseDto
    {
        
        public string Message { get; set; } = "";
        public dynamic? Data { get; set; }

        public ResponseDto Notify(string Text)
        {
            Message = Text;
            return this;
        }
    }

    public class ResponseAdapterDto
    {
        public string Message { get; set; } = "";
        public dynamic? Data { get; set; }
        public bool IsSuccess { get; set; } = false;
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; } = "";

        public ResponseAdapterDto Notify(string text)
        {
            Message = text;
            return this;
        }

        public ResponseAdapterDto SetError(int statusCode, string errorMessage)
        {
            IsSuccess = false;
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
            Message = (statusCode > 400 && statusCode < 408 && errorMessage.Length < 255 && errorMessage.Length > 0) ? errorMessage : (statusCode >= 500 && statusCode != 504 && errorMessage.Length < 255 && errorMessage.Length > 0) ? errorMessage : GetDefaultErrorMessageForStatusCode(statusCode) ;
            return this;
        }

        public ResponseAdapterDto SetSuccess(int statusCode, dynamic? data, string successMessage)
        {
            IsSuccess = true;
            StatusCode = statusCode;
            Data = data;
            Message = successMessage;
            return this;
        }

        private string GetDefaultErrorMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 400:
                    return "Bad Request: The server cannot process the request due to a client error";
                case 401:
                    return "Unauthorized: Authentication is required";
                case 403:
                    return "Forbidden: Access is denied";
                case 404:
                    return "Not Found: The requested resource was not found";
                case 405:
                    return "Method Not Allowed: The method is not supported for the requested resource";
                case 408:
                    return "Request Timeout: The server timed out waiting for the request";
                case 500:
                    return "Internal Server Error: The server encountered an unexpected condition";
                case 501:
                    return "Not Implemented: The server does not support the functionality required";
                case 502:
                    return "Bad Gateway: Invalid response from an upstream server";
                case 503:
                    return "Service Unavailable: The server is currently unable to handle the request";
                case 504:
                    return "Gateway Timeout: The server did not receive a timely response from an upstream server";
                default:
                    return "An unknown error has occurred";
            }
        }
    }


}
