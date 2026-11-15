using MarcasAutos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MarcasAutos.Tests.Helpers;

internal static class DbContextFactory
{
    public static AppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"MarcasAutos-{Guid.NewGuid()}").Options;
        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}