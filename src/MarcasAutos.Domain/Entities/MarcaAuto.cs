namespace MarcasAutos.Domain.Entities;

/// <summary>Representa una marca de automóviles del catálogo del dominio.</summary>
public sealed class MarcaAuto
{
    /// <summary>Identificador único de la marca.</summary>
    public int Id { get; set; }

    /// <summary>Nombre comercial con el que se identifica la marca.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>País donde se originó la marca, cuando esta información está disponible.</summary>
    public string? PaisOrigen { get; set; }

    /// <summary>Año en que se fundó la marca.</summary>
    public int AnioFundacion { get; set; }

    /// <summary>Indica si la marca se encuentra activa en el catálogo.</summary>
    public bool Activa { get; set; }
}
