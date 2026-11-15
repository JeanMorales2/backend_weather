using MarcasAutos.Infrastructure.Adapters.In.Http;
using MarcasAutos.Infrastructure.Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.Swagger;

namespace MarcasAutos.Tests.Infrastructure.Framework;

/// <summary>Verifica que el adaptador web registra descubrimiento de controladores, Swagger y health checks sin iniciar la API.</summary>
public sealed class WebFrameworkExtensionsTests
{
    [Fact]
    public void AddWebFramework_RegistersWebServicesAndControllerAssembly()
    {
        // Se usa el builder real de ASP.NET Core: ya trae logging y entorno de hosting, que el API Explorer
        // necesita al resolverse. No se inicia el servidor, solo se construye el contenedor de dependencias.
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddWebFramework();

        using var app = builder.Build();
        var provider = app.Services;
        Assert.IsAssignableFrom<IApiDescriptionGroupCollectionProvider>(provider.GetRequiredService<IApiDescriptionGroupCollectionProvider>());
        Assert.IsAssignableFrom<ISwaggerProvider>(provider.GetRequiredService<ISwaggerProvider>());
        Assert.IsAssignableFrom<HealthCheckService>(provider.GetRequiredService<HealthCheckService>());
        var partManager = provider.GetRequiredService<ApplicationPartManager>();
        Assert.Contains(partManager.ApplicationParts, part => part is AssemblyPart assemblyPart && assemblyPart.Assembly == typeof(MarcasAutosController).Assembly);
    }
}

