using MarcasAutos.Domain.Entities;
using MarcasAutos.Infrastructure.Adapters.Out.Persistence;
using MarcasAutos.Tests.Helpers;

namespace MarcasAutos.Tests.Infrastructure.Adapters.Out.Persistence;

/// <summary>Verifica el adaptador EF Core con una base InMemory aislada para cada prueba.</summary>
public sealed class MarcaAutoRepositoryTests
{
    [Fact]
    public async Task ListarOrdenadasPorNombreAsync_ReturnsSeededBrandsOrdered()
    {
        await using var db = DbContextFactory.Create();
        var results = await new MarcaAutoRepository(db).ListarOrdenadasPorNombreAsync();
        Assert.Equal(
            ["Audi", "BMW", "Chevrolet", "Fiat", "Ford", "Honda", "Hyundai", "Kia", "Mazda", "Mercedes-Benz", "Mitsubishi", "Nissan", "Peugeot", "Renault", "Subaru", "Suzuki", "Tesla", "Toyota", "Volkswagen", "Volvo"],
            results.Select(x => x.Nombre));
    }

    [Fact]
    public async Task ObtenerPorIdAsync_ReturnsBrandWhenItExists()
    {
        await using var db = DbContextFactory.Create();
        var result = await new MarcaAutoRepository(db).ObtenerPorIdAsync(1);
        Assert.NotNull(result);
        Assert.Equal("Toyota", result.Nombre);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_ReturnsNullWhenMissing()
    {
        await using var db = DbContextFactory.Create();
        Assert.Null(await new MarcaAutoRepository(db).ObtenerPorIdAsync(999));
    }
}
