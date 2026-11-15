using MarcasAutos.Domain.Entities;

namespace MarcasAutos.Application.Ports.Out;

/// <summary>
/// Define el contrato de persistencia que necesita la aplicación; no conoce EF Core,
/// PostgreSQL ni ningún detalle del adaptador que lo implementa.
/// </summary>
public interface IMarcaAutoRepository
{
    /// <summary>Obtiene todas las marcas ordenadas alfabéticamente por su nombre.</summary>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Las entidades de dominio recuperadas del catálogo.</returns>
    Task<IReadOnlyList<MarcaAuto>> ListarOrdenadasPorNombreAsync(CancellationToken cancellationToken = default);

    /// <summary>Obtiene una marca por su identificador.</summary>
    /// <param name="id">Identificador de la marca.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>La entidad encontrada o <see langword="null"/> si no existe.</returns>
    Task<MarcaAuto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
}
