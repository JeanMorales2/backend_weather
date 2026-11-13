using MarcasAutos.Api.Services;
using MarcasAutos.Tests.Helpers;

namespace MarcasAutos.Tests.Services;

public sealed class MarcasAutosServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsSeededBrandsOrderedAndMapped()
    {
        await using var db = DbContextFactory.Create();
        var results = await new MarcasAutosService(db).GetAllAsync();

        Assert.Equal(6, results.Count);
        Assert.Equal(
            ["Chevrolet", "Ford", "Honda", "Nissan", "Toyota", "Volkswagen"],
            results.Select(brand => brand.Nombre));
        Assert.Equal("Japón", results.Single(x => x.Nombre == "Honda").PaisOrigen);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsBrandWhenItExists()
    {
        await using var db = DbContextFactory.Create();
        var result = await new MarcasAutosService(db).GetByIdAsync(1);
        Assert.NotNull(result);
        Assert.Equal("Toyota", result.Nombre);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullWhenMissing()
    {
        await using var db = DbContextFactory.Create();
        Assert.Null(await new MarcasAutosService(db).GetByIdAsync(999));
    }
}
