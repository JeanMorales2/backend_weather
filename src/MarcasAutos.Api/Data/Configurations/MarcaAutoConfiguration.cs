using MarcasAutos.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarcasAutos.Api.Data.Configurations;

public sealed class MarcaAutoConfiguration : IEntityTypeConfiguration<MarcaAuto>
{
    public void Configure(EntityTypeBuilder<MarcaAuto> builder)
    {
        builder.ToTable("MarcasAutos");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.Nombre).IsUnique();
        builder.Property(x => x.PaisOrigen).HasMaxLength(80);
        // HasData permite que la migración inicial deje un catálogo útil desde el primer arranque.
        builder.HasData(
            new MarcaAuto { Id = 1, Nombre = "Toyota", PaisOrigen = "Japón", AnioFundacion = 1937, Activa = true },
            new MarcaAuto
            {
                Id = 2, Nombre = "Ford", PaisOrigen = "Estados Unidos", AnioFundacion = 1903, Activa = true
            },
            new MarcaAuto
            {
                Id = 3, Nombre = "Volkswagen", PaisOrigen = "Alemania", AnioFundacion = 1937, Activa = true
            },
            new MarcaAuto { Id = 4, Nombre = "Honda", PaisOrigen = "Japón", AnioFundacion = 1948, Activa = true },
            new MarcaAuto
            {
                Id = 5, Nombre = "Chevrolet", PaisOrigen = "Estados Unidos", AnioFundacion = 1911, Activa = true
            },
            new MarcaAuto { Id = 6, Nombre = "Nissan", PaisOrigen = "Japón", AnioFundacion = 1933, Activa = true });
    }
}
