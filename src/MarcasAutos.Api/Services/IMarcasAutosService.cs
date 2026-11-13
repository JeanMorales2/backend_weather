using MarcasAutos.Api.Dtos;

namespace MarcasAutos.Api.Services;

public interface IMarcasAutosService
{
    Task<IReadOnlyList<MarcaAutoDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MarcaAutoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
