using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class Imagen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imagen_Publicaciones_PublicacionId",
                table: "Imagen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Imagen",
                table: "Imagen");

            migrationBuilder.RenameTable(
                name: "Imagen",
                newName: "Imagenes");

            migrationBuilder.RenameIndex(
                name: "IX_Imagen_PublicacionId",
                table: "Imagenes",
                newName: "IX_Imagenes_PublicacionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Imagenes",
                table: "Imagenes",
                column: "ImagenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Imagenes_Publicaciones_PublicacionId",
                table: "Imagenes",
                column: "PublicacionId",
                principalTable: "Publicaciones",
                principalColumn: "PublicacionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imagenes_Publicaciones_PublicacionId",
                table: "Imagenes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Imagenes",
                table: "Imagenes");

            migrationBuilder.RenameTable(
                name: "Imagenes",
                newName: "Imagen");

            migrationBuilder.RenameIndex(
                name: "IX_Imagenes_PublicacionId",
                table: "Imagen",
                newName: "IX_Imagen_PublicacionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Imagen",
                table: "Imagen",
                column: "ImagenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Imagen_Publicaciones_PublicacionId",
                table: "Imagen",
                column: "PublicacionId",
                principalTable: "Publicaciones",
                principalColumn: "PublicacionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
