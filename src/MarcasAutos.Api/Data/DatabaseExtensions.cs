using Microsoft.EntityFrameworkCore;

namespace MarcasAutos.Api.Data;

public static class DatabaseExtensions
{
    private const int MaxAttempts = 10;
    private static readonly TimeSpan DefaultRetryDelay = TimeSpan.FromSeconds(3);

    public static Task ApplyMigrationsAsync(this IHost host) =>
        host.ApplyMigrationsAsync(DefaultRetryDelay);

    public static async Task ApplyMigrationsAsync(this IHost host, TimeSpan retryDelay)
    {
        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseMigrations");

        await ApplyMigrationsWithRetryAsync(
            () => db.Database.MigrateAsync(),
            logger,
            retryDelay);
    }

    // En Docker la API puede arrancar antes de que PostgreSQL acepte conexiones;
    // se reintenta unas cuantas veces y, si sigue fallando, se deja caer el arranque.
    internal static async Task ApplyMigrationsWithRetryAsync(
        Func<Task> migrate,
        ILogger logger,
        TimeSpan delay)
    {
        var attempt = 1;
        while (true)
        {
            try
            {
                await migrate();
                return;
            }
            catch (Exception exception) when (attempt < MaxAttempts)
            {
                attempt++;
                logger.LogWarning(
                    exception,
                    "No se pudieron aplicar las migraciones. Reintento {Attempt}/{MaxAttempts}.",
                    attempt,
                    MaxAttempts);
                await Task.Delay(delay);
            }
        }
    }
}
