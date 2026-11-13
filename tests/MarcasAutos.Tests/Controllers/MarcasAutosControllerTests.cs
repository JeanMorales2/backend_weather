using MarcasAutos.Api.Controllers;
using MarcasAutos.Api.Dtos;
using MarcasAutos.Api.Services;
using MarcasAutos.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MarcasAutos.Tests.Controllers;

public sealed class MarcasAutosControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsOkWithBrands()
    {
        await using var db = DbContextFactory.Create();
        var controller = new MarcasAutosController(new MarcasAutosService(db));

        var response = await controller.GetAll(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(response.Result);
        Assert.Equal(6, Assert.IsAssignableFrom<IReadOnlyList<MarcaAutoDto>>(ok.Value).Count);
    }

    [Fact]
    public async Task GetById_ReturnsOkWhenFound()
    {
        await using var db = DbContextFactory.Create();
        var response = await new MarcasAutosController(new MarcasAutosService(db)).GetById(1, CancellationToken.None);
        Assert.IsType<OkObjectResult>(response.Result);
    }

    [Fact]
    public async Task GetById_ReturnsNotFoundWhenMissing()
    {
        await using var db = DbContextFactory.Create();
        var response = await new MarcasAutosController(new MarcasAutosService(db)).GetById(999, CancellationToken.None);
        Assert.IsType<NotFoundResult>(response.Result);
    }
}
