using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLegacyUsuarioIdColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Itens");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Locais");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Itens",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Categorias",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Locais",
                type: "text",
                nullable: true);
        }
    }
}
