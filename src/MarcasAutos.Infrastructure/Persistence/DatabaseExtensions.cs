using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MarcasAutos.Infrastructure.Persistence;

/// <summary>Contiene el arranque seguro de las migraciones del adaptador de persistencia.</summary>
public static class DatabaseExtensions
{
    private const int MaxAttempts = 10;
    private static readonly TimeSpan DefaultRetryDelay = TimeSpan.FromSeconds(3);

    /// <summary>Aplica las migraciones con el intervalo de reintento predeterminado.</summary>
    public static Task ApplyMigrationsAsync(this IHost host) => host.ApplyMigrationsAsync(DefaultRetryDelay);

    /// <summary>Aplica las migraciones creando un ámbito propio para los servicios con duración <c>scoped</c>.</summary>
    public static async Task ApplyMigrationsAsync(this IHost host, TimeSpan retryDelay)
    {
        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseMigrations");
        await ApplyMigrationsWithRetryAsync(() => db.Database.MigrateAsync(), logger, retryDelay);
    }

    /// <summary>Ejecuta la migración con reintentos para tolerar la disponibilidad tardía de la base de datos.</summary>
    internal static async Task ApplyMigrationsWithRetryAsync(Func<Task> migrate, ILogger logger, TimeSpan delay)
    {
        var attempt = 1;
        while (true)
        {
            try { await migrate(); return; }
            // En Docker la API puede iniciar antes de que PostgreSQL acepte conexiones.
            catch (Exception exception) when (attempt < MaxAttempts)
            {
                attempt++;
                logger.LogWarning(exception, "No se pudieron aplicar las migraciones. Reintento {Attempt}/{MaxAttempts}.", attempt, MaxAttempts);
                await Task.Delay(delay);
            }
            // Al agotarse los intentos no se captura la excepción: el arranque debe fallar visiblemente.
        }
    }
}
