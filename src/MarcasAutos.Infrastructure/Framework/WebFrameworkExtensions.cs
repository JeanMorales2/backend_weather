using MarcasAutos.Infrastructure.Adapters.In.Http;
using MarcasAutos.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace MarcasAutos.Infrastructure.Framework;

/// <summary>Registra los servicios web que convierten HTTP en llamadas a los puertos de entrada.</summary>
public static class WebFrameworkExtensions
{
    /// <summary>Agrega controladores, documentación OpenAPI y verificación de disponibilidad de la base de datos.</summary>
    public static IServiceCollection AddWebFramework(this IServiceCollection services)
    {
        // El controlador está en Infrastructure; ApplicationPart lo descubre desde el ensamblado web anfitrión.
        services.AddControllers().AddApplicationPart(typeof(MarcasAutosController).Assembly);
        services.AddEndpointsApiExplorer();
        // El título del documento se fija aquí; si no, Swagger usaría el nombre del ensamblado de arranque (MarcasAutos.Main).
        services.AddSwaggerGen(options =>
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "MarcasAutos API", Version = "1.0" }));
        services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
        return services;
    }
}
