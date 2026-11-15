using MarcasAutos.Application.Dtos;

namespace MarcasAutos.Application.Ports.In;

/// <summary>Puerto de entrada para consultar una marca mediante su identificador.</summary>
public interface IObtenerMarcaAutoPorIdUseCase
{
    /// <summary>Busca una marca sin exponer detalles del mecanismo de persistencia.</summary>
    /// <param name="id">Identificador de la marca buscada.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>La marca encontrada o <see langword="null"/> si no existe.</returns>
    Task<MarcaAutoDto?> ExecuteAsync(int id, CancellationToken cancellationToken = default);
}
