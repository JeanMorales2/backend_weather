using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MarcasAutos.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AmpliarCatalogoMarcas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MarcasAutos",
                columns: new[] { "Id", "Activa", "AnioFundacion", "Nombre", "PaisOrigen" },
                values: new object[,]
                {
                    { 7, true, 1916, "BMW", "Alemania" },
                    { 8, true, 1926, "Mercedes-Benz", "Alemania" },
                    { 9, true, 1909, "Audi", "Alemania" },
                    { 10, true, 1967, "Hyundai", "Corea del Sur" },
                    { 11, true, 1944, "Kia", "Corea del Sur" },
                    { 12, true, 1920, "Mazda", "Japón" },
                    { 13, true, 1953, "Subaru", "Japón" },
                    { 14, true, 1909, "Suzuki", "Japón" },
                    { 15, true, 1870, "Mitsubishi", "Japón" },
                    { 16, true, 1899, "Renault", "Francia" },
                    { 17, true, 1810, "Peugeot", "Francia" },
                    { 18, true, 1899, "Fiat", "Italia" },
                    { 19, true, 1927, "Volvo", "Suecia" },
                    { 20, true, 2003, "Tesla", "Estados Unidos" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 20);
        }
    }
}
