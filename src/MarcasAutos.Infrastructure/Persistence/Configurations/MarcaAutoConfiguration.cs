using MarcasAutos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarcasAutos.Infrastructure.Persistence.Configurations;

/// <summary>Define el esquema relacional y los datos iniciales de <see cref="MarcaAuto"/>.</summary>
public sealed class MarcaAutoConfiguration : IEntityTypeConfiguration<MarcaAuto>
{
    /// <summary>Configura las restricciones que la base de datos debe garantizar para una marca.</summary>
    public void Configure(EntityTypeBuilder<MarcaAuto> builder)
    {
        // Se fija el nombre de tabla para que el esquema sea explícito e independiente de convenciones.
        builder.ToTable("MarcasAutos");
        builder.HasKey(x => x.Id);
        // Los límites protegen la calidad de los datos y se corresponden con el esquema PostgreSQL.
        builder.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
        // Dos registros no deben representar la misma marca comercial.
        builder.HasIndex(x => x.Nombre).IsUnique();
        builder.Property(x => x.PaisOrigen).HasMaxLength(80);
        // Datos semilla: 20 marcas (la prueba exige al menos tres) con claves estables, para que el catálogo
        // sea idéntico en cada arranque. Las seis primeras viajan en la migración inicial y las demás en
        // AmpliarCatalogoMarcas, de modo que una base ya migrada se actualiza sin perder datos.
        builder.HasData(
            new MarcaAuto { Id = 1, Nombre = "Toyota", PaisOrigen = "Japón", AnioFundacion = 1937, Activa = true },
            new MarcaAuto { Id = 2, Nombre = "Ford", PaisOrigen = "Estados Unidos", AnioFundacion = 1903, Activa = true },
            new MarcaAuto { Id = 3, Nombre = "Volkswagen", PaisOrigen = "Alemania", AnioFundacion = 1937, Activa = true },
            new MarcaAuto { Id = 4, Nombre = "Honda", PaisOrigen = "Japón", AnioFundacion = 1948, Activa = true },
            new MarcaAuto { Id = 5, Nombre = "Chevrolet", PaisOrigen = "Estados Unidos", AnioFundacion = 1911, Activa = true },
            new MarcaAuto { Id = 6, Nombre = "Nissan", PaisOrigen = "Japón", AnioFundacion = 1933, Activa = true },
            new MarcaAuto { Id = 7, Nombre = "BMW", PaisOrigen = "Alemania", AnioFundacion = 1916, Activa = true },
            new MarcaAuto { Id = 8, Nombre = "Mercedes-Benz", PaisOrigen = "Alemania", AnioFundacion = 1926, Activa = true },
            new MarcaAuto { Id = 9, Nombre = "Audi", PaisOrigen = "Alemania", AnioFundacion = 1909, Activa = true },
            new MarcaAuto { Id = 10, Nombre = "Hyundai", PaisOrigen = "Corea del Sur", AnioFundacion = 1967, Activa = true },
            new MarcaAuto { Id = 11, Nombre = "Kia", PaisOrigen = "Corea del Sur", AnioFundacion = 1944, Activa = true },
            new MarcaAuto { Id = 12, Nombre = "Mazda", PaisOrigen = "Japón", AnioFundacion = 1920, Activa = true },
            new MarcaAuto { Id = 13, Nombre = "Subaru", PaisOrigen = "Japón", AnioFundacion = 1953, Activa = true },
            new MarcaAuto { Id = 14, Nombre = "Suzuki", PaisOrigen = "Japón", AnioFundacion = 1909, Activa = true },
            new MarcaAuto { Id = 15, Nombre = "Mitsubishi", PaisOrigen = "Japón", AnioFundacion = 1870, Activa = true },
            new MarcaAuto { Id = 16, Nombre = "Renault", PaisOrigen = "Francia", AnioFundacion = 1899, Activa = true },
            new MarcaAuto { Id = 17, Nombre = "Peugeot", PaisOrigen = "Francia", AnioFundacion = 1810, Activa = true },
            new MarcaAuto { Id = 18, Nombre = "Fiat", PaisOrigen = "Italia", AnioFundacion = 1899, Activa = true },
            new MarcaAuto { Id = 19, Nombre = "Volvo", PaisOrigen = "Suecia", AnioFundacion = 1927, Activa = true },
            new MarcaAuto { Id = 20, Nombre = "Tesla", PaisOrigen = "Estados Unidos", AnioFundacion = 2003, Activa = true });
    }
}
