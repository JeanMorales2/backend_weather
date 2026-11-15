using MarcasAutos.Application.Dtos;
using MarcasAutos.Application.Ports.In;
using MarcasAutos.Infrastructure.Adapters.In.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarcasAutos.Tests.Infrastructure.Adapters.In.Http;

/// <summary>Verifica las respuestas HTTP del controlador usando puertos de entrada falsos.</summary>
public sealed class MarcasAutosControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsOkWithBrands()
    {
        var controller = new MarcasAutosController(new ListarFake(Brands), new ObtenerFake(Brands));
        var response = await controller.GetAll(CancellationToken.None);
        var ok = Assert.IsType<OkObjectResult>(response.Result);
        Assert.Equal(6, Assert.IsAssignableFrom<IReadOnlyList<MarcaAutoDto>>(ok.Value).Count);
    }

    [Fact]
    public async Task GetById_ReturnsOkWhenFound()
    {
        var response = await new MarcasAutosController(new ListarFake(Brands), new ObtenerFake(Brands)).GetById(1, CancellationToken.None);
        Assert.IsType<OkObjectResult>(response.Result);
    }

    [Fact]
    public async Task GetById_ReturnsNotFoundWhenMissing()
    {
        var response = await new MarcasAutosController(new ListarFake(Brands), new ObtenerFake(Brands)).GetById(999, CancellationToken.None);
        Assert.IsType<NotFoundResult>(response.Result);
    }

    private static readonly MarcaAutoDto[] Brands =
    [
        new(1, "Toyota", "Japón", 1937, true), new(2, "Ford", "Estados Unidos", 1903, true),
        new(3, "Volkswagen", "Alemania", 1937, true), new(4, "Honda", "Japón", 1948, true),
        new(5, "Chevrolet", "Estados Unidos", 1911, true), new(6, "Nissan", "Japón", 1933, true)
    ];

    private sealed class ListarFake(IReadOnlyList<MarcaAutoDto> marcas) : IListarMarcasAutosUseCase
    {
        public Task<IReadOnlyList<MarcaAutoDto>> ExecuteAsync(CancellationToken cancellationToken = default) => Task.FromResult(marcas);
    }

    private sealed class ObtenerFake(IReadOnlyList<MarcaAutoDto> marcas) : IObtenerMarcaAutoPorIdUseCase
    {
        public Task<MarcaAutoDto?> ExecuteAsync(int id, CancellationToken cancellationToken = default) =>
            Task.FromResult(marcas.SingleOrDefault(x => x.Id == id));
    }
}
