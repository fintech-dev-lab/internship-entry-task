using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TicTacToe.API.Attributes;

namespace TicTacToe.Api.SwaggerFilters;

public class IdempotencyKeyHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasIdempotencyAttribute = context.MethodInfo.GetCustomAttributes(typeof(IdempotencyRequiredAttribute), false).Any();

        if (!hasIdempotencyAttribute)
            return;
        
        operation.Parameters ??= [];
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Idempotency-Key",
            In = ParameterLocation.Header,
            Description = "Уникальный ключ для обеспечения идемпотентности запроса.",
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Format = "uuid"
            }
        });
    }
}