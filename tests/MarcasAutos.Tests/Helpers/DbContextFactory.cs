using MarcasAutos.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace MarcasAutos.Tests.Helpers;

internal static class DbContextFactory
{
    public static AppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"MarcasAutos-{Guid.NewGuid()}").Options;
        var context = new AppDbContext(options);
        // Cada prueba usa una base aislada y EnsureCreated aplica también los datos de HasData.
        context.Database.EnsureCreated();
        return context;
    }
}
