using MarcasAutos.Application.Dtos;

namespace MarcasAutos.Application.Ports.In;

/// <summary>Puerto de entrada para consultar el catálogo completo de marcas.</summary>
public interface IListarMarcasAutosUseCase
{
    /// <summary>Obtiene las marcas disponibles en el orden definido por el caso de uso.</summary>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Una vista inmutable de las marcas para el adaptador que invocó el caso de uso.</returns>
    Task<IReadOnlyList<MarcaAutoDto>> ExecuteAsync(CancellationToken cancellationToken = default);
}
