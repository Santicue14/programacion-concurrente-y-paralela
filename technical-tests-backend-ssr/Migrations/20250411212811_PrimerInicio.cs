using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace technical_tests_backend_ssr.Migrations
{
    /// <inheritdoc />
    public partial class PrimerInicio : Migration
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
                name: "Marcas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marcas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Modelos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MarcaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modelos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modelos_Marcas_MarcaId",
                        column: x => x.MarcaId,
                        principalTable: "Marcas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ModeloId = table.Column<int>(type: "int", nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehiculos_Modelos_ModeloId",
                        column: x => x.ModeloId,
                        principalTable: "Modelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                table: "Marcas",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Toyota" },
                    { 2, "Ford" },
                    { 3, "Chevrolet" },
                    { 4, "Honda" },
                    { 5, "Nissan" },
                    { 6, "Hyundai" },
                    { 7, "Volkswagen" },
                    { 8, "BMW" },
                    { 9, "Audi" },
                    { 10, "Mercedes-Benz" },
                    { 11, "Renault" },
                    { 12, "Peugeot" },
                    { 13, "Fiat" }
                });

            migrationBuilder.InsertData(
                table: "Modelos",
                columns: new[] { "Id", "MarcaId", "Nombre" },
                values: new object[,]
                {
                    { 1, 1, "Corolla" },
                    { 2, 2, "Focus" },
                    { 3, 3, "Cruze" },
                    { 4, 4, "Civic" },
                    { 5, 5, "Sentra" },
                    { 6, 6, "Elantra" },
                    { 7, 7, "Golf" },
                    { 8, 8, "Serie 3" },
                    { 9, 9, "A4" },
                    { 10, 10, "Clase C" },
                    { 11, 11, "Kangoo" },
                    { 12, 12, "208" },
                    { 13, 13, "Cronos" }
                });

            migrationBuilder.InsertData(
                table: "Vehiculos",
                columns: new[] { "Id", "Anio", "ModeloId", "Precio", "Stock" },
                values: new object[,]
                {
                    { 1, 2020, 1, 5100000m, 10 },
                    { 2, 2019, 2, 6300000m, 8 },
                    { 3, 2021, 3, 5500000m, 12 },
                    { 4, 2022, 4, 7000000m, 5 },
                    { 5, 2018, 5, 5200000m, 15 },
                    { 6, 2020, 6, 5600000m, 7 },
                    { 7, 2021, 7, 6500000m, 6 },
                    { 8, 2019, 8, 8000000m, 4 },
                    { 9, 2022, 9, 9000000m, 3 },
                    { 10, 2020, 10, 8500000m, 2 },
                    { 11, 2021, 11, 5100000m, 10 },
                    { 12, 2022, 12, 6200000m, 9 },
                    { 13, 2023, 13, 5900000m, 11 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Modelos_MarcaId",
                table: "Modelos",
                column: "MarcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_ModeloId",
                table: "Vehiculos",
                column: "ModeloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Vehiculos");

            migrationBuilder.DropTable(
                name: "Modelos");

            migrationBuilder.DropTable(
                name: "Marcas");
        }
    }
}
