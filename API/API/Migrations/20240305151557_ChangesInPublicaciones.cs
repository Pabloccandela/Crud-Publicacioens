using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInPublicaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imagen_Propiedades_PublicacionId",
                table: "Imagen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Propiedades",
                table: "Propiedades");

            migrationBuilder.RenameTable(
                name: "Propiedades",
                newName: "Publicaciones");

            migrationBuilder.AlterColumn<int>(
                name: "TipoOperacion",
                table: "Publicaciones",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "TipoPropiedad",
                table: "Publicaciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Publicaciones",
                table: "Publicaciones",
                column: "PublicacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Imagen_Publicaciones_PublicacionId",
                table: "Imagen",
                column: "PublicacionId",
                principalTable: "Publicaciones",
                principalColumn: "PublicacionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imagen_Publicaciones_PublicacionId",
                table: "Imagen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Publicaciones",
                table: "Publicaciones");

            migrationBuilder.DropColumn(
                name: "TipoPropiedad",
                table: "Publicaciones");

            migrationBuilder.RenameTable(
                name: "Publicaciones",
                newName: "Propiedades");

            migrationBuilder.AlterColumn<string>(
                name: "TipoOperacion",
                table: "Propiedades",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Propiedades",
                table: "Propiedades",
                column: "PublicacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Imagen_Propiedades_PublicacionId",
                table: "Imagen",
                column: "PublicacionId",
                principalTable: "Propiedades",
                principalColumn: "PublicacionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
