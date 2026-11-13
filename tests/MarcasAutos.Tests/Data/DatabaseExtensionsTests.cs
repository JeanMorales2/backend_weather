using MarcasAutos.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MarcasAutos.Tests.Data;

public sealed class DatabaseExtensionsTests
{
    [Fact]
    public async Task ApplyMigrationsWithRetryAsync_SucceedsOnFirstAttempt()
    {
        var attempts = 0;
        using var loggerFactory = LoggerFactory.Create(logging => logging.ClearProviders());

        await DatabaseExtensions.ApplyMigrationsWithRetryAsync(
            () =>
            {
                attempts++;
                return Task.CompletedTask;
            },
            loggerFactory.CreateLogger("Tests"),
            TimeSpan.Zero);

        Assert.Equal(1, attempts);
    }

    [Fact]
    public async Task ApplyMigrationsWithRetryAsync_RecoversAfterTransientFailures()
    {
        var attempts = 0;
        var logger = new WarningCountingLogger();

        await DatabaseExtensions.ApplyMigrationsWithRetryAsync(
            () =>
            {
                attempts++;
                if (attempts <= 2)
                {
                    throw new InvalidOperationException("Error transitorio.");
                }

                return Task.CompletedTask;
            },
            logger,
            TimeSpan.Zero);

        Assert.Equal(3, attempts);
        Assert.Equal(2, logger.WarningCount);
    }

    [Fact]
    public async Task ApplyMigrationsAsync_PropagatesErrorWhenProviderCannotMigrate()
    {
        using var host = Host.CreateDefaultBuilder()
            .ConfigureLogging(logging => logging.ClearProviders())
            .ConfigureServices(services => services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString())))
            .Build();

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => host.ApplyMigrationsAsync(TimeSpan.Zero));

        Assert.Contains("Relational-specific", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ApplyMigrationsWithRetryAsync_PropagatesFailureAfterMaximumAttempts()
    {
        var attempts = 0;
        using var loggerFactory = LoggerFactory.Create(logging => logging.ClearProviders());

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => DatabaseExtensions.ApplyMigrationsWithRetryAsync(
                () =>
                {
                    attempts++;
                    throw new InvalidOperationException("Error de migración.");
                },
                loggerFactory.CreateLogger("Tests"),
                TimeSpan.Zero));

        Assert.Equal("Error de migración.", exception.Message);
        Assert.Equal(10, attempts);
    }

    private sealed class WarningCountingLogger : ILogger
    {
        public int WarningCount { get; private set; }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (logLevel == LogLevel.Warning)
            {
                WarningCount++;
            }
        }
    }
}
