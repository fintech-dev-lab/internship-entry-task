using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using TicTacToe.Data;
using Microsoft.AspNetCore.Identity;
using TicTacToe.Services;
using TicTacToe.Abstractions;
using TicTacToe.Models;
using TicTacToe.Validators;
using TicTacToe.Middlewares;

namespace TicTacToe.Extentions
{
    public static class ServicesExtensions
    {
        public static WebApplicationBuilder AddData(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationContext>(opt =>
               opt.UseInMemoryDatabase("TestDB"));
            return builder;
        }

        public static WebApplicationBuilder AddControllers(this WebApplicationBuilder builder)
        {
            builder.Services.AddRouting(opt => opt.LowercaseUrls = true);
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            return builder;
        }

        public static WebApplicationBuilder AddFluentValidation(this WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
            builder.Services.AddFluentValidationAutoValidation(config =>
            {
                config.DisableDataAnnotationsValidation = true;
            });
            return builder;
        }

        public static WebApplicationBuilder AddAutoMapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new AutoMapperProfile()));
            return builder;
        }

        public static WebApplicationBuilder AddAppServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IGameService, GameService>();
            return builder;
        }

        public static WebApplicationBuilder AddExceptionHandler(this WebApplicationBuilder builder)
        {
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            return builder;
        }

        public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<GameOptions>(builder.Configuration.GetSection("GameOptions"));
            return builder;
        }

        public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Task API",
                    Version = "v1"
                });
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TicTacToe.xml"));
            });

            return builder;
        }
    }
}
