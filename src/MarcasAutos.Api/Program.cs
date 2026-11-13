using MarcasAutos.Api.Data;
using MarcasAutos.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("No se configuró ConnectionStrings:Default.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, postgres => postgres.EnableRetryOnFailure()));
builder.Services.AddScoped<IMarcasAutosService, MarcasAutosService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

var app = builder.Build();

await app.ApplyMigrationsAsync();

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
