using MarcasAutos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MarcasAutos.Tests.Infrastructure.Persistence;

/// <summary>
/// Verifica <c>ApplyMigrationsAsync(IHost, TimeSpan)</c> con un host real pero sin PostgreSQL: el proveedor EF InMemory
/// no soporta migraciones relacionales, así que <c>MigrateAsync</c> falla siempre y se puede comprobar el ciclo completo
/// (ámbito, resolución del DbContext y del logger, reintentos y error final) sin esperas reales (retraso cero).
/// </summary>
public sealed class DatabaseExtensionsHostTests
{
    [Fact]
    public async Task ApplyMigrationsAsync_WithHost_RetriesAndRethrowsWhenMigrationsKeepFailing()
    {
        var logs = new CapturingLoggerProvider();
        var builder = Host.CreateApplicationBuilder();
        builder.Logging.ClearProviders();
        builder.Logging.AddProvider(logs);
        builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase($"MarcasAutos-Host-{Guid.NewGuid()}"));
        using var host = builder.Build();

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => host.ApplyMigrationsAsync(TimeSpan.Zero));

        // Tras agotar los 10 intentos el error no se oculta: el arranque debe fallar visiblemente.
        Assert.Contains("Relational", exception.Message);
        // Entre el primer intento y el décimo se registran 9 advertencias de reintento.
        Assert.Equal(9, logs.Warnings.Count(message => message.Contains("Reintento")));
    }

    [Fact]
    public async Task ApplyMigrationsAsync_WithDefaultDelay_RunsMigrationOnceWhenItSucceeds()
    {
        // Proveedor relacional (Npgsql) para que MigrateAsync sea válido, pero con el migrador sustituido por uno falso:
        // así se prueba el camino de éxito (incluida la sobrecarga con el retraso predeterminado) sin conectar a PostgreSQL.
        var builder = Host.CreateApplicationBuilder();
        builder.Logging.ClearProviders();
        builder.Services.AddDbContext<AppDbContext>(options => options
            .UseNpgsql("Host=localhost;Database=pruebas;Username=pruebas;Password=pruebas")
            .ReplaceService<IMigrator, RecordingMigrator>());
        using var host = builder.Build();
        RecordingMigrator.Calls = 0;

        // Al tener éxito en el primer intento no hay esperas, aunque el retraso predeterminado sea de 3 segundos.
        await host.ApplyMigrationsAsync();

        Assert.Equal(1, RecordingMigrator.Calls);
    }

    /// <summary>Migrador falso que solo cuenta cuántas veces se le pidió migrar.</summary>
    private sealed class RecordingMigrator : IMigrator
    {
        public static int Calls { get; set; }

        public void Migrate(string? targetMigration = null) => Calls++;

        public Task MigrateAsync(string? targetMigration = null, CancellationToken cancellationToken = default)
        {
            Calls++;
            return Task.CompletedTask;
        }

        public bool HasPendingModelChanges() => false;

        public string GenerateScript(string? fromMigration = null, string? toMigration = null, MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
            => string.Empty;
    }

    /// <summary>Proveedor de logging mínimo que guarda los mensajes de advertencia para poder comprobarlos.</summary>
    private sealed class CapturingLoggerProvider : ILoggerProvider
    {
        public List<string> Warnings { get; } = [];

        public ILogger CreateLogger(string categoryName) => new CapturingLogger(Warnings);

        public void Dispose()
        {
        }

        private sealed class CapturingLogger(List<string> warnings) : ILogger
        {
            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                if (logLevel == LogLevel.Warning)
                {
                    warnings.Add(formatter(state, exception));
                }
            }
        }
    }
}
