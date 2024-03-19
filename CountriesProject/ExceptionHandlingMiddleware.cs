using CountriesProject.Models;
using Microsoft.Extensions.Logging.Configuration;
using System.Net;


namespace CountriesProject
{
    public class ExceptionHandlingMiddleware( ILogger<ExceptionHandlingMiddleware> logger ): IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate _next)
        {
            try
            {
                logger.LogInformation("Before request");
                await _next(context);
                logger.LogInformation("After request");
            }
            catch (Exception ex)
            {                
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            logger.LogError(exception, "An error occurred.");                       

            ExceptionResponse response = exception switch
            {
                ApplicationException _ => new ExceptionResponse(HttpStatusCode.BadRequest, "Application exception occurred."),
                KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, "The request key not found."),
                UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized."),
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
            };                   

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.ErrorCode;
            await context.Response.WriteAsJsonAsync(response);    
            
        }
    }

}
