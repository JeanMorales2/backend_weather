namespace MarcasAutos.Api.Dtos;

public sealed record MarcaAutoDto(int Id, string Nombre, string? PaisOrigen, int AnioFundacion, bool Activa);
