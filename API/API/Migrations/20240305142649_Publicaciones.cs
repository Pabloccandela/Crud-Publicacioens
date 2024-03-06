using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class Publicaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Propiedades",
                columns: table => new
                {
                    PublicacionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoOperacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ambientes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    M2 = table.Column<double>(type: "float", nullable: false),
                    Antiguedad = table.Column<int>(type: "int", nullable: false),
                    CoordenadasUbicacion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propiedades", x => x.PublicacionId);
                });

            migrationBuilder.CreateTable(
                name: "Imagen",
                columns: table => new
                {
                    ImagenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ruta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contenido = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PublicacionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagen", x => x.ImagenId);
                    table.ForeignKey(
                        name: "FK_Imagen_Propiedades_PublicacionId",
                        column: x => x.PublicacionId,
                        principalTable: "Propiedades",
                        principalColumn: "PublicacionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Imagen_PublicacionId",
                table: "Imagen",
                column: "PublicacionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Imagen");

            migrationBuilder.DropTable(
                name: "Propiedades");
        }
    }
}
