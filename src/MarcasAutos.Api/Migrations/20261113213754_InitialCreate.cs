using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MarcasAutos.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarcasAutos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PaisOrigen = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    AnioFundacion = table.Column<int>(type: "integer", nullable: false),
                    Activa = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarcasAutos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MarcasAutos",
                columns: new[] { "Id", "Activa", "AnioFundacion", "Nombre", "PaisOrigen" },
                values: new object[,]
                {
                    { 1, true, 1937, "Toyota", "Japón" },
                    { 2, true, 1903, "Ford", "Estados Unidos" },
                    { 3, true, 1937, "Volkswagen", "Alemania" },
                    { 4, true, 1948, "Honda", "Japón" },
                    { 5, true, 1911, "Chevrolet", "Estados Unidos" },
                    { 6, true, 1933, "Nissan", "Japón" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarcasAutos_Nombre",
                table: "MarcasAutos",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarcasAutos");
        }
    }
}
