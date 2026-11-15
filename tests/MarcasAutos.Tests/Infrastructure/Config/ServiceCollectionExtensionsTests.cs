using MarcasAutos.Application.Ports.In;
using MarcasAutos.Application.Ports.Out;
using MarcasAutos.Application.UseCases;
using MarcasAutos.Infrastructure.Adapters.Out.Persistence;
using MarcasAutos.Infrastructure.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarcasAutos.Tests.Infrastructure.Config;

/// <summary>Verifica que el cableado de Application e Infrastructure resuelve los puertos sin abrir PostgreSQL.</summary>
public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddApplicationAndAddInfrastructure_RegisterExpectedImplementations()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = "Host=localhost;Database=marcasautos;Username=test;Password=test"
            })
            .Build();
        var services = new ServiceCollection();

        services.AddApplication();
        services.AddInfrastructure(configuration);

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        Assert.IsType<ListarMarcasAutos>(scope.ServiceProvider.GetRequiredService<IListarMarcasAutosUseCase>());
        Assert.IsType<ObtenerMarcaAutoPorId>(scope.ServiceProvider.GetRequiredService<IObtenerMarcaAutoPorIdUseCase>());
        Assert.IsType<MarcaAutoRepository>(scope.ServiceProvider.GetRequiredService<IMarcaAutoRepository>());
    }

    [Fact]
    public void AddInfrastructure_ThrowsWhenDefaultConnectionStringIsMissing()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>()).Build();

        var exception = Assert.Throws<InvalidOperationException>(() => services.AddInfrastructure(configuration));

        Assert.Equal("No se configuró ConnectionStrings:Default.", exception.Message);
    }
}

