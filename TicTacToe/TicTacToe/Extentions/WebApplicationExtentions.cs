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
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>(); 

            int maxRetryAttempts = 5; 
            TimeSpan retryDelay = TimeSpan.FromSeconds(3);

            for (int attempt = 1; attempt <= maxRetryAttempts; attempt++)
            {
                try
                {
                    logger.LogInformation("Applying migrations (Attempt {Attempt}/{MaxAttempts})...", attempt, maxRetryAttempts);
                    dbContext.Database.Migrate();
                    logger.LogInformation("Migrations applied successfully!");
                    return app;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Migration attempt {Attempt}/{MaxAttempts} failed.", attempt, maxRetryAttempts);

                    if (attempt == maxRetryAttempts)
                    {
                        logger.LogCritical("All migration attempts failed. Application will exit.");
                        throw;
                    }

                    Thread.Sleep(retryDelay);
                }
            }

            return app;
        }
    }
}
