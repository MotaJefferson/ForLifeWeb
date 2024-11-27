using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForLifeWeb.Migrations
{
    /// <inheritdoc />
    public partial class AdicaoVencimentoEstimado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "data_vencimento_estimado",
                table: "InsumoEstoque",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data_vencimento_estimado",
                table: "InsumoEstoque");
        }
    }
}
