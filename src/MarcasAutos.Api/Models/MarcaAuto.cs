namespace MarcasAutos.Api.Models;

public sealed class MarcaAuto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? PaisOrigen { get; set; }
    public int AnioFundacion { get; set; }
    public bool Activa { get; set; }
}
