using Microsoft.EntityFrameworkCore;

namespace MarcasAutos.Api.Data;

public static class DatabaseExtensions
{
    public static async Task ApplyMigrationsAsync(
        this IHost host,
        TimeSpan? retryDelay = null)
    {
        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseMigrations");
        var delay = retryDelay ?? TimeSpan.FromSeconds(3);

        await ApplyMigrationsWithRetryAsync(
            () => db.Database.MigrateAsync(),
            logger,
            delay);
    }

    internal static async Task ApplyMigrationsWithRetryAsync(
        Func<Task> migrate,
        ILogger logger,
        TimeSpan delay)
    {
        for (var attempt = 1; attempt <= 10; attempt++)
        {
            try
            {
                await migrate();
                return;
            }
            catch (Exception exception) when (attempt < 10)
            {
                logger.LogWarning(
                    exception,
                    "No se pudieron aplicar las migraciones. Reintento {Attempt}/10.",
                    attempt + 1);
                await Task.Delay(delay);
            }
        }
    }
}
