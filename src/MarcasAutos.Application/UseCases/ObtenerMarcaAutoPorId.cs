using MarcasAutos.Application.Dtos;
using MarcasAutos.Application.Ports.In;
using MarcasAutos.Application.Ports.Out;

namespace MarcasAutos.Application.UseCases;

/// <summary>Implementa la búsqueda de una marca y adapta la entidad encontrada a un DTO.</summary>
public sealed class ObtenerMarcaAutoPorId(IMarcaAutoRepository repository) : IObtenerMarcaAutoPorIdUseCase
{
    /// <summary>Devuelve la marca solicitada o <see langword="null"/> cuando no existe.</summary>
    public async Task<MarcaAutoDto?> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var marca = await repository.ObtenerPorIdAsync(id, cancellationToken);
        return marca is null ? null : new MarcaAutoDto(
            marca.Id, marca.Nombre, marca.PaisOrigen, marca.AnioFundacion, marca.Activa);
    }
}
