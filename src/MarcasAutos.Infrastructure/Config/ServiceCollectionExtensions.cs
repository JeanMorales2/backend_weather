using MarcasAutos.Application.Ports.In;
using MarcasAutos.Application.Ports.Out;
using MarcasAutos.Application.UseCases;
using MarcasAutos.Infrastructure.Adapters.Out.Persistence;
using MarcasAutos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarcasAutos.Infrastructure.Config;

/// <summary>Registra los casos de uso y adaptadores que materializan los puertos de la arquitectura hexagonal.</summary>
public static class ServiceCollectionExtensions
{
    /// <summary>Agrega las implementaciones de los puertos de entrada de la capa Application.</summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IListarMarcasAutosUseCase, ListarMarcasAutos>();
        services.AddScoped<IObtenerMarcaAutoPorIdUseCase, ObtenerMarcaAutoPorId>();
        return services;
    }

    /// <summary>Agrega EF Core con PostgreSQL y el adaptador de persistencia que implementa el puerto de salida.</summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("No se configuró ConnectionStrings:Default.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString, postgres => postgres.EnableRetryOnFailure()));
        services.AddScoped<IMarcaAutoRepository, MarcaAutoRepository>();
        return services;
    }
}
