using MarcasAutos.Application.Dtos;
using MarcasAutos.Application.Ports.In;
using MarcasAutos.Application.Ports.Out;

namespace MarcasAutos.Application.UseCases;

/// <summary>Implementa la consulta del catálogo y transforma entidades de dominio en DTOs de salida.</summary>
public sealed class ListarMarcasAutos(IMarcaAutoRepository repository) : IListarMarcasAutosUseCase
{
    /// <summary>Consulta el repositorio y proyecta sus entidades al contrato de la aplicación.</summary>
    public async Task<IReadOnlyList<MarcaAutoDto>> ExecuteAsync(CancellationToken cancellationToken = default) =>
        (await repository.ListarOrdenadasPorNombreAsync(cancellationToken)).Select(Map).ToArray();

    private static MarcaAutoDto Map(Domain.Entities.MarcaAuto marca) =>
        new(marca.Id, marca.Nombre, marca.PaisOrigen, marca.AnioFundacion, marca.Activa);
}
