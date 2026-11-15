using MarcasAutos.Application.Dtos;
using MarcasAutos.Application.Ports.In;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarcasAutos.Infrastructure.Adapters.In.Http;

/// <summary>Adaptador HTTP que expone las consultas de marcas sin acoplarse a la persistencia.</summary>
[ApiController]
[Route("api/[controller]")]
public sealed class MarcasAutosController(
    IListarMarcasAutosUseCase listarMarcasAutos,
    IObtenerMarcaAutoPorIdUseCase obtenerMarcaAutoPorId) : ControllerBase
{
    /// <summary>Devuelve todas las marcas ordenadas por nombre.</summary>
    /// <param name="cancellationToken">Token para cancelar la consulta.</param>
    /// <returns>La lista de marcas disponibles.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MarcaAutoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MarcaAutoDto>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await listarMarcasAutos.ExecuteAsync(cancellationToken));

    /// <summary>Busca una marca por su identificador.</summary>
    /// <param name="id">Identificador de la marca.</param>
    /// <param name="cancellationToken">Token para cancelar la consulta.</param>
    /// <returns>La marca encontrada o una respuesta 404.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MarcaAutoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MarcaAutoDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var marca = await obtenerMarcaAutoPorId.ExecuteAsync(id, cancellationToken);
        return marca is null ? NotFound() : Ok(marca);
    }
}
