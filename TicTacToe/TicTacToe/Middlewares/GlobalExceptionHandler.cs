using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;

namespace TicTacToe.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, exception.Message);

            var options = httpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            var serializeSettings = httpContext.RequestServices.GetRequiredService<IOptions<MvcNewtonsoftJsonOptions>>().Value.SerializerSettings;

            var problemDetails = new ProblemDetails()
            {
                Type = options.Value.ClientErrorMapping[500].Link,
                Title = "Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };
            problemDetails.Extensions.Add("traceId", Activity.Current?.Id ?? httpContext.TraceIdentifier);

            var resposne = JsonConvert.SerializeObject(problemDetails, serializeSettings);

            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsync(resposne, cancellationToken);
            return true;
        }
    }
}
