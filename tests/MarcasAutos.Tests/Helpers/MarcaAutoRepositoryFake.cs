using MarcasAutos.Application.Ports.Out;
using MarcasAutos.Domain.Entities;

namespace MarcasAutos.Tests.Helpers;

internal sealed class MarcaAutoRepositoryFake(IEnumerable<MarcaAuto> marcas) : IMarcaAutoRepository
{
    private readonly IReadOnlyList<MarcaAuto> marcas = marcas.ToArray();

    public Task<IReadOnlyList<MarcaAuto>> ListarOrdenadasPorNombreAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<MarcaAuto>>(marcas.OrderBy(x => x.Nombre).ToArray());

    public Task<MarcaAuto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default) =>
        Task.FromResult(marcas.SingleOrDefault(x => x.Id == id));
}