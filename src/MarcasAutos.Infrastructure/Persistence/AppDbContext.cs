using MarcasAutos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarcasAutos.Infrastructure.Persistence;

/// <summary>Contexto de EF Core del adaptador de persistencia para el catálogo de marcas.</summary>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>Conjunto de marcas persistidas en la base de datos.</summary>
    public DbSet<MarcaAuto> MarcasAutos => Set<MarcaAuto>();

    /// <summary>Aplica todas las configuraciones de entidades declaradas en este ensamblado.</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
