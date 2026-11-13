using MarcasAutos.Api.Data;
using MarcasAutos.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MarcasAutos.Api.Services;

public sealed class MarcasAutosService(AppDbContext context) : IMarcasAutosService
{
    public async Task<IReadOnlyList<MarcaAutoDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await context.MarcasAutos.AsNoTracking().OrderBy(x => x.Nombre)
            .Select(x => new MarcaAutoDto(x.Id, x.Nombre, x.PaisOrigen, x.AnioFundacion, x.Activa))
            .ToListAsync(cancellationToken);

    public async Task<MarcaAutoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await context.MarcasAutos.AsNoTracking().Where(x => x.Id == id)
            .Select(x => new MarcaAutoDto(x.Id, x.Nombre, x.PaisOrigen, x.AnioFundacion, x.Activa))
            .FirstOrDefaultAsync(cancellationToken);
}
