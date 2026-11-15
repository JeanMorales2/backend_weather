namespace MarcasAutos.Application.Dtos;

/// <summary>Representa los datos de una marca que la aplicación expone a sus adaptadores de entrada.</summary>
/// <param name="Id">Identificador único de la marca.</param>
/// <param name="Nombre">Nombre comercial de la marca.</param>
/// <param name="PaisOrigen">País de origen, si se conoce.</param>
/// <param name="AnioFundacion">Año de fundación de la marca.</param>
/// <param name="Activa">Indica si la marca está activa.</param>
public sealed record MarcaAutoDto(int Id, string Nombre, string? PaisOrigen, int AnioFundacion, bool Activa);
