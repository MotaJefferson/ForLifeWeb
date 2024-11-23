using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForLifeWeb.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCampoAtivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ativo",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ativo",
                table: "Produtos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ativo",
                table: "Clientes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ativo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ativo",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "ativo",
                table: "Clientes");
        }
    }
}
