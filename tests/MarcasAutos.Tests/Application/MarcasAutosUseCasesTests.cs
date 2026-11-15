using MarcasAutos.Application.UseCases;
using MarcasAutos.Domain.Entities;
using MarcasAutos.Tests.Helpers;

namespace MarcasAutos.Tests.Application;

/// <summary>Verifica los casos de uso con un repositorio falso para aislar las reglas de Application.</summary>
public sealed class MarcasAutosUseCasesTests
{
    [Fact]
    public async Task Listar_ReturnsSeededBrandsOrderedAndMapped()
    {
        var results = await new ListarMarcasAutos(new MarcaAutoRepositoryFake(Brands)).ExecuteAsync();

        Assert.Equal(6, results.Count);
        Assert.Equal(["Chevrolet", "Ford", "Honda", "Nissan", "Toyota", "Volkswagen"], results.Select(brand => brand.Nombre));
        Assert.Equal("Japón", results.Single(x => x.Nombre == "Honda").PaisOrigen);
    }

    [Fact]
    public async Task ObtenerPorId_ReturnsBrandWhenItExists()
    {
        var result = await new ObtenerMarcaAutoPorId(new MarcaAutoRepositoryFake(Brands)).ExecuteAsync(1);
        Assert.NotNull(result);
        Assert.Equal("Toyota", result.Nombre);
    }

    [Fact]
    public async Task ObtenerPorId_ReturnsNullWhenMissing() =>
        Assert.Null(await new ObtenerMarcaAutoPorId(new MarcaAutoRepositoryFake(Brands)).ExecuteAsync(999));

    private static readonly MarcaAuto[] Brands =
    [
        new() { Id = 1, Nombre = "Toyota", PaisOrigen = "Japón", AnioFundacion = 1937, Activa = true },
        new() { Id = 2, Nombre = "Ford", PaisOrigen = "Estados Unidos", AnioFundacion = 1903, Activa = true },
        new() { Id = 3, Nombre = "Volkswagen", PaisOrigen = "Alemania", AnioFundacion = 1937, Activa = true },
        new() { Id = 4, Nombre = "Honda", PaisOrigen = "Japón", AnioFundacion = 1948, Activa = true },
        new() { Id = 5, Nombre = "Chevrolet", PaisOrigen = "Estados Unidos", AnioFundacion = 1911, Activa = true },
        new() { Id = 6, Nombre = "Nissan", PaisOrigen = "Japón", AnioFundacion = 1933, Activa = true }
    ];
}
