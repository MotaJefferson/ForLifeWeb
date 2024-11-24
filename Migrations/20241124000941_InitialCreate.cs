using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForLifeWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    cpf = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    endereco = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.id_cliente);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    id_fornecedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cpf = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    razao_social = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    cnpj = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    telefone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    endereco = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.id_fornecedor);
                });

            migrationBuilder.CreateTable(
                name: "Insumos",
                columns: table => new
                {
                    id_insumo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    periodo_vencimento = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insumos", x => x.id_insumo);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cargo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    cpf = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    cod_usuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    senha = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    data_cadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "InsumoCompra",
                columns: table => new
                {
                    id_compra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fornecedor_id = table.Column<int>(type: "int", nullable: false),
                    insumo_id = table.Column<int>(type: "int", nullable: false),
                    preco_unitario = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    valor_compra = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsumoCompra", x => x.id_compra);
                    table.ForeignKey(
                        name: "FK_InsumoCompra_Fornecedores_fornecedor_id",
                        column: x => x.fornecedor_id,
                        principalTable: "Fornecedores",
                        principalColumn: "id_fornecedor",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InsumoCompra_Insumos_insumo_id",
                        column: x => x.insumo_id,
                        principalTable: "Insumos",
                        principalColumn: "id_insumo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InsumoEstoque",
                columns: table => new
                {
                    id_estoque = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fornecedor_id = table.Column<int>(type: "int", nullable: false),
                    insumo_id = table.Column<int>(type: "int", nullable: false),
                    quantidade_atual = table.Column<int>(type: "int", nullable: true),
                    quantidade_entrada = table.Column<int>(type: "int", nullable: true),
                    quantidade_saida = table.Column<int>(type: "int", nullable: true),
                    data_entrada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    data_saida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    data_baixa = table.Column<DateTime>(type: "datetime2", nullable: true),
                    data_registro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    data_vencimento_estimado = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsumoEstoque", x => x.id_estoque);
                    table.ForeignKey(
                        name: "FK_InsumoEstoque_Fornecedores_fornecedor_id",
                        column: x => x.fornecedor_id,
                        principalTable: "Fornecedores",
                        principalColumn: "id_fornecedor",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InsumoEstoque_Insumos_insumo_id",
                        column: x => x.insumo_id,
                        principalTable: "Insumos",
                        principalColumn: "id_insumo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    id_produto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    insumo_id = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    periodo_vencimento = table.Column<int>(type: "int", nullable: false),
                    periodo_colheita = table.Column<int>(type: "int", nullable: false),
                    periodo_limite_colheita = table.Column<int>(type: "int", nullable: true),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.id_produto);
                    table.ForeignKey(
                        name: "FK_Produtos_Insumos_insumo_id",
                        column: x => x.insumo_id,
                        principalTable: "Insumos",
                        principalColumn: "id_insumo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Plantio",
                columns: table => new
                {
                    id_plantio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    insumo_id = table.Column<int>(type: "int", nullable: false),
                    produto_id = table.Column<int>(type: "int", nullable: false),
                    quantidade_plantio = table.Column<int>(type: "int", nullable: false),
                    data_plantio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    data_colheita = table.Column<DateTime>(type: "datetime2", nullable: true),
                    data_vencimento_estimado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    data_registro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    data_baixa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plantio", x => x.id_plantio);
                    table.ForeignKey(
                        name: "FK_Plantio_Insumos_insumo_id",
                        column: x => x.insumo_id,
                        principalTable: "Insumos",
                        principalColumn: "id_insumo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plantio_Produtos_produto_id",
                        column: x => x.produto_id,
                        principalTable: "Produtos",
                        principalColumn: "id_produto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoEstoque",
                columns: table => new
                {
                    id_estoque = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    produto_id = table.Column<int>(type: "int", nullable: false),
                    quantidade_atual = table.Column<int>(type: "int", nullable: false),
                    quantidade_saida = table.Column<int>(type: "int", nullable: true),
                    quantidade_colheita = table.Column<int>(type: "int", nullable: true),
                    data_colheita = table.Column<DateTime>(type: "datetime2", nullable: true),
                    data_saida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    data_vencimento_estimado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    data_registro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    data_baixa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoEstoque", x => x.id_estoque);
                    table.ForeignKey(
                        name: "FK_ProdutoEstoque_Produtos_produto_id",
                        column: x => x.produto_id,
                        principalTable: "Produtos",
                        principalColumn: "id_produto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vendas",
                columns: table => new
                {
                    id_venda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    produto_id = table.Column<int>(type: "int", nullable: false),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    numero_venda = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    data_registro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    quantidade_venda = table.Column<int>(type: "int", nullable: false),
                    data_venda = table.Column<DateTime>(type: "datetime2", nullable: false),
                    preco_unitario = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    valor_venda = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    forma_pagamento = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendas", x => x.id_venda);
                    table.ForeignKey(
                        name: "FK_Vendas_Clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "Clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vendas_Produtos_produto_id",
                        column: x => x.produto_id,
                        principalTable: "Produtos",
                        principalColumn: "id_produto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InsumoCompra_fornecedor_id",
                table: "InsumoCompra",
                column: "fornecedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_InsumoCompra_insumo_id",
                table: "InsumoCompra",
                column: "insumo_id");

            migrationBuilder.CreateIndex(
                name: "IX_InsumoEstoque_fornecedor_id",
                table: "InsumoEstoque",
                column: "fornecedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_InsumoEstoque_insumo_id",
                table: "InsumoEstoque",
                column: "insumo_id");

            migrationBuilder.CreateIndex(
                name: "IX_Plantio_insumo_id",
                table: "Plantio",
                column: "insumo_id");

            migrationBuilder.CreateIndex(
                name: "IX_Plantio_produto_id",
                table: "Plantio",
                column: "produto_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoEstoque_produto_id",
                table: "ProdutoEstoque",
                column: "produto_id");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_insumo_id",
                table: "Produtos",
                column: "insumo_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_cliente_id",
                table: "Vendas",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_produto_id",
                table: "Vendas",
                column: "produto_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsumoCompra");

            migrationBuilder.DropTable(
                name: "InsumoEstoque");

            migrationBuilder.DropTable(
                name: "Plantio");

            migrationBuilder.DropTable(
                name: "ProdutoEstoque");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropTable(
                name: "Fornecedores");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Insumos");
        }
    }
}
