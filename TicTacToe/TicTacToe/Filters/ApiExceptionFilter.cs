using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TicTacToe.Exceptions;

namespace TicTacToe.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly IOptions<ApiBehaviorOptions> _options;
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger, IOptions<ApiBehaviorOptions> options)
        {
            _options = options;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            if (exception is not ServiceException serviceException)
                throw exception;

            var response = new ProblemDetails()
            {
                Title = serviceException.Title,
                Detail = exception.Message,
                Status = serviceException.StatusCode,
                Type = _options.Value.ClientErrorMapping[serviceException.StatusCode].Link,
                Instance = context.HttpContext.Request.Path,
            };

            _logger.LogWarning("Api method {path} finished with code {statusCode} and error: {error}",
                             context.HttpContext.Request.Path, serviceException.StatusCode, response.Detail);

            context.Result = new JsonResult(response) { StatusCode = serviceException.StatusCode };
        }
    }
}
