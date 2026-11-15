using MarcasAutos.Application.Ports.Out;
using MarcasAutos.Domain.Entities;
using MarcasAutos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MarcasAutos.Infrastructure.Adapters.Out.Persistence;

/// <summary>
/// Adaptador de salida que implementa el puerto de persistencia con EF Core y el proveedor Npgsql.
/// Las entidades se devuelven separadas del seguimiento; los casos de uso realizan la proyección a DTO.
/// </summary>
public sealed class MarcaAutoRepository(AppDbContext context) : IMarcaAutoRepository
{
    /// <summary>
    /// Lee el catálogo ordenado por nombre. <c>AsNoTracking</c> evita el coste de seguimiento
    /// porque esta consulta es solo de lectura.
    /// </summary>
    public async Task<IReadOnlyList<MarcaAuto>> ListarOrdenadasPorNombreAsync(CancellationToken cancellationToken = default) =>
        await context.MarcasAutos.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(cancellationToken);

    /// <summary>Busca una marca sin seguimiento para no mantener estado de una consulta de lectura.</summary>
    public Task<MarcaAuto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default) =>
        context.MarcasAutos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}
