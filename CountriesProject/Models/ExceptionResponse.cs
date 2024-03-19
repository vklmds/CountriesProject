using System.Net;

namespace CountriesProject.Models
{
    public class ExceptionResponse
    { 
        public HttpStatusCode ErrorCode { get; set; }

        public string? Message { get; set; }

        public ExceptionResponse(HttpStatusCode errorCode, string? message)
        {
            ErrorCode = errorCode;  
            Message = message;
        }
    }
}
