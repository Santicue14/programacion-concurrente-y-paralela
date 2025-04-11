using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace technical_tests_backend_ssr.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Apellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Marca = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Modelo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "Id", "Apellido", "Email", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, "Cuevas", "santiagobcuevas14@gmail.com", "Santiago", "1161970490" },
                    { 2, "Pérez", "lucia.perez@email.com", "Lucía", "1134567890" },
                    { 3, "Gómez", "martin.gomez@email.com", "Martín", "1145678901" },
                    { 4, "Rodríguez", "camila.rodriguez@email.com", "Camila", "1156789012" },
                    { 5, "Fernández", "julian.fernandez@email.com", "Julián", "1167890123" },
                    { 6, "López", "sofia.lopez@email.com", "Sofía", "1178901234" },
                    { 7, "Díaz", "mateo.diaz@email.com", "Mateo", "1189012345" },
                    { 8, "Martínez", "valentina.martinez@email.com", "Valentina", "1190123456" },
                    { 9, "Sosa", "tomas.sosa@email.com", "Tomás", "1101234567" },
                    { 10, "Herrera", "agustina.herrera@email.com", "Agustina", "1112345678" }
                });

            migrationBuilder.InsertData(
                table: "Vehiculos",
                columns: new[] { "Id", "Anio", "Marca", "Modelo", "Precio", "Stock" },
                values: new object[,]
                {
                    { 1, 2020, "Toyota", "Corolla", 5100000m, 10 },
                    { 2, 2019, "Ford", "Focus", 6300000m, 8 },
                    { 3, 2021, "Chevrolet", "Cruze", 5500000m, 12 },
                    { 4, 2022, "Honda", "Civic", 7000000m, 5 },
                    { 5, 2018, "Nissan", "Sentra", 5200000m, 15 },
                    { 6, 2020, "Hyundai", "Elantra", 5600000m, 7 },
                    { 7, 2021, "Volkswagen", "Golf", 6500000m, 6 },
                    { 8, 2019, "BMW", "Serie 3", 8000000m, 4 },
                    { 9, 2022, "Audi", "A4", 9000000m, 3 },
                    { 10, 2020, "Mercedes-Benz", "Clase C", 8500000m, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Vehiculos");
        }
    }
}
