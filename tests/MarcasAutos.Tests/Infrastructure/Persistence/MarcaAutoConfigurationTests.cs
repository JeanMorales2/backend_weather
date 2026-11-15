using MarcasAutos.Domain.Entities;
using MarcasAutos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MarcasAutos.Tests.Infrastructure.Persistence;

/// <summary>Verifica el esquema y la siembra mediante EF InMemory sin requerir PostgreSQL.</summary>
public sealed class MarcaAutoConfigurationTests
{
    [Fact]
    public void Model_MapsMarcaAutoWithExpectedConstraintsAndSeedData()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using var context = new AppDbContext(options);
        var entity = context.Model.FindEntityType(typeof(MarcaAuto));
        Assert.NotNull(entity);
        Assert.Equal("MarcasAutos", entity.GetTableName());
        var nombre = entity.FindProperty(nameof(MarcaAuto.Nombre));
        Assert.NotNull(nombre);
        Assert.False(nombre.IsNullable);
        Assert.Equal(100, nombre.GetMaxLength());
        var paisOrigen = entity.FindProperty(nameof(MarcaAuto.PaisOrigen));
        Assert.NotNull(paisOrigen);
        Assert.Equal(80, paisOrigen.GetMaxLength());
        var nombreIndex = Assert.Single(entity.GetIndexes(), index => index.Properties.Count == 1 && index.Properties[0].Name == nameof(MarcaAuto.Nombre));
        Assert.True(nombreIndex.IsUnique);
        var designEntity = context.GetService<IDesignTimeModel>().Model.FindEntityType(typeof(MarcaAuto));
        Assert.NotNull(designEntity);
        var seedIds = designEntity.GetSeedData().Select(seed => Assert.IsType<int>(seed[nameof(MarcaAuto.Id)])).Order().ToArray();
        // El catálogo semilla trae 20 marcas con claves 1..20 (6 de la migración inicial + 14 de AmpliarCatalogoMarcas).
        Assert.Equal(Enumerable.Range(1, 20), seedIds);
        // Y ningún nombre se repite, porque el índice único lo rechazaría al migrar.
        var seedNames = designEntity.GetSeedData().Select(seed => (string)seed[nameof(MarcaAuto.Nombre)]!).ToArray();
        Assert.Equal(seedNames.Length, seedNames.Distinct().Count());
    }
}
