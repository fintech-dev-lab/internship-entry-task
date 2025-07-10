using TicTacToe.Extentions;

namespace TicTacToe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.AddData();
            builder.AddControllers();
            builder.AddAutoMapper();
            builder.AddAppServices();
            builder.AddExceptionHandler();
            builder.AddSwagger();
            builder.AddFluentValidation();
            builder.AddOptions();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
