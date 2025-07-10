using Microsoft.EntityFrameworkCore;
using TicTacToe.Data;

namespace TicTacToe.Extentions
{
    public static class WebApplicationExtentions
    {
        public static WebApplication MigrateDB(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            dbContext.Database.Migrate();
            return app;
        }
    }
}
