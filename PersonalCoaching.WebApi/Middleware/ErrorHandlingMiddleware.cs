using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersonalCoaching.WebApi.GlobalExceptionHandling
{
    public class ErrorHandlingMiddleware //Global Exception Handling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _logger = serviceProvider.GetRequiredService<ILogger<ErrorHandlingMiddleware>>(); ;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Process the request normally
            }
            catch (Exception ex)
            {
                // Log when errors occur.
                _logger.LogError(ex, "An error has occurred.");

                // Return the error message in JSON format.
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var errorRespponse = new
                {
                    message = "A error has occurred. Please try again later.",
                    error = ex.Message
                };

                //Post the error message as a response in JSON format.
                await context.Response.WriteAsJsonAsync(errorRespponse);
            }
        }
    }
}
