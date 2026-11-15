using MarcasAutos.Infrastructure.Config;
using MarcasAutos.Infrastructure.Framework;
using MarcasAutos.Infrastructure.Persistence;

// Main es la raíz de composición: une los puertos de Application con los adaptadores de Infrastructure.
var builder = WebApplication.CreateBuilder(args);

// No se anuncia el servidor en la cabecera HTTP "Server": reduce la información expuesta sin afectar a la API.
builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

// El arranque registra configuración y servicios antes de construir la aplicación.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebFramework();

var app = builder.Build();

// Antes de exponer endpoints se actualiza el esquema; después se habilitan Swagger, health checks y controladores.
await app.ApplyMigrationsAsync();

// Swagger puede activarse por configuración y, si no se especifica, solo se muestra durante Development.
var swaggerEnabled = builder.Configuration.GetValue<bool?>("Swagger:Enabled")
    ?? app.Environment.IsDevelopment();

if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");
app.MapControllers();
app.Run();

public partial class Program;
