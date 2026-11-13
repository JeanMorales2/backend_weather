using MarcasAutos.Api.Data;
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
}
