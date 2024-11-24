IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Clientes] (
    [id_cliente] int NOT NULL IDENTITY,
    [nome] nvarchar(100) NOT NULL,
    [telefone] nvarchar(15) NULL,
    [cpf] nvarchar(15) NOT NULL,
    [endereco] nvarchar(500) NULL,
    [ativo] bit NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([id_cliente])
);

CREATE TABLE [Fornecedores] (
    [id_fornecedor] int NOT NULL IDENTITY,
    [nome] nvarchar(100) NOT NULL,
    [cpf] nvarchar(15) NULL,
    [razao_social] nvarchar(100) NULL,
    [cnpj] nvarchar(20) NULL,
    [telefone] nvarchar(15) NULL,
    [endereco] nvarchar(500) NULL,
    CONSTRAINT [PK_Fornecedores] PRIMARY KEY ([id_fornecedor])
);

CREATE TABLE [Insumos] (
    [id_insumo] int NOT NULL IDENTITY,
    [nome] nvarchar(100) NOT NULL,
    [descricao] nvarchar(500) NULL,
    [tipo] nvarchar(50) NOT NULL,
    [ativo] bit NOT NULL,
    [periodo_vencimento] int NULL,
    CONSTRAINT [PK_Insumos] PRIMARY KEY ([id_insumo])
);

CREATE TABLE [Usuarios] (
    [id_usuario] int NOT NULL IDENTITY,
    [nome] nvarchar(100) NOT NULL,
    [cargo] nvarchar(50) NOT NULL,
    [cpf] nvarchar(15) NOT NULL,
    [cod_usuario] nvarchar(50) NOT NULL,
    [senha] nvarchar(100) NOT NULL,
    [data_cadastro] datetime2 NOT NULL,
    [ativo] bit NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([id_usuario])
);

CREATE TABLE [InsumoCompra] (
    [id_compra] int NOT NULL IDENTITY,
    [fornecedor_id] int NOT NULL,
    [insumo_id] int NOT NULL,
    [preco_unitario] decimal(18,4) NOT NULL,
    [quantidade] int NOT NULL,
    [valor_compra] decimal(18,4) NULL,
    CONSTRAINT [PK_InsumoCompra] PRIMARY KEY ([id_compra]),
    CONSTRAINT [FK_InsumoCompra_Fornecedores_fornecedor_id] FOREIGN KEY ([fornecedor_id]) REFERENCES [Fornecedores] ([id_fornecedor]) ON DELETE NO ACTION,
    CONSTRAINT [FK_InsumoCompra_Insumos_insumo_id] FOREIGN KEY ([insumo_id]) REFERENCES [Insumos] ([id_insumo]) ON DELETE NO ACTION
);

CREATE TABLE [InsumoEstoque] (
    [id_estoque] int NOT NULL IDENTITY,
    [fornecedor_id] int NOT NULL,
    [insumo_id] int NOT NULL,
    [quantidade_atual] int NULL,
    [quantidade_entrada] int NULL,
    [quantidade_saida] int NULL,
    [data_entrada] datetime2 NULL,
    [data_saida] datetime2 NULL,
    [data_baixa] datetime2 NULL,
    [data_registro] datetime2 NULL,
    [data_vencimento_estimado] datetime2 NULL,
    CONSTRAINT [PK_InsumoEstoque] PRIMARY KEY ([id_estoque]),
    CONSTRAINT [FK_InsumoEstoque_Fornecedores_fornecedor_id] FOREIGN KEY ([fornecedor_id]) REFERENCES [Fornecedores] ([id_fornecedor]) ON DELETE NO ACTION,
    CONSTRAINT [FK_InsumoEstoque_Insumos_insumo_id] FOREIGN KEY ([insumo_id]) REFERENCES [Insumos] ([id_insumo]) ON DELETE NO ACTION
);

CREATE TABLE [Produtos] (
    [id_produto] int NOT NULL IDENTITY,
    [insumo_id] int NOT NULL,
    [nome] nvarchar(100) NOT NULL,
    [descricao] nvarchar(500) NOT NULL,
    [periodo_vencimento] int NOT NULL,
    [periodo_colheita] int NOT NULL,
    [periodo_limite_colheita] int NULL,
    [ativo] bit NOT NULL,
    CONSTRAINT [PK_Produtos] PRIMARY KEY ([id_produto]),
    CONSTRAINT [FK_Produtos_Insumos_insumo_id] FOREIGN KEY ([insumo_id]) REFERENCES [Insumos] ([id_insumo]) ON DELETE NO ACTION
);

CREATE TABLE [Plantio] (
    [id_plantio] int NOT NULL IDENTITY,
    [insumo_id] int NOT NULL,
    [produto_id] int NOT NULL,
    [quantidade_plantio] int NOT NULL,
    [data_plantio] datetime2 NULL,
    [data_colheita] datetime2 NULL,
    [data_vencimento_estimado] datetime2 NULL,
    [data_registro] datetime2 NULL,
    [data_baixa] datetime2 NULL,
    CONSTRAINT [PK_Plantio] PRIMARY KEY ([id_plantio]),
    CONSTRAINT [FK_Plantio_Insumos_insumo_id] FOREIGN KEY ([insumo_id]) REFERENCES [Insumos] ([id_insumo]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Plantio_Produtos_produto_id] FOREIGN KEY ([produto_id]) REFERENCES [Produtos] ([id_produto]) ON DELETE NO ACTION
);

CREATE TABLE [ProdutoEstoque] (
    [id_estoque] int NOT NULL IDENTITY,
    [produto_id] int NOT NULL,
    [quantidade_atual] int NOT NULL,
    [quantidade_saida] int NULL,
    [quantidade_colheita] int NULL,
    [data_colheita] datetime2 NULL,
    [data_saida] datetime2 NULL,
    [data_vencimento_estimado] datetime2 NULL,
    [data_registro] datetime2 NULL,
    [data_baixa] datetime2 NULL,
    CONSTRAINT [PK_ProdutoEstoque] PRIMARY KEY ([id_estoque]),
    CONSTRAINT [FK_ProdutoEstoque_Produtos_produto_id] FOREIGN KEY ([produto_id]) REFERENCES [Produtos] ([id_produto]) ON DELETE NO ACTION
);

CREATE TABLE [Vendas] (
    [id_venda] int NOT NULL IDENTITY,
    [produto_id] int NOT NULL,
    [cliente_id] int NOT NULL,
    [numero_venda] nvarchar(50) NULL,
    [data_registro] datetime2 NULL,
    [quantidade_venda] int NOT NULL,
    [data_venda] datetime2 NOT NULL,
    [preco_unitario] decimal(18,4) NOT NULL,
    [valor_venda] decimal(18,4) NOT NULL,
    [forma_pagamento] nvarchar(10) NULL,
    CONSTRAINT [PK_Vendas] PRIMARY KEY ([id_venda]),
    CONSTRAINT [FK_Vendas_Clientes_cliente_id] FOREIGN KEY ([cliente_id]) REFERENCES [Clientes] ([id_cliente]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Vendas_Produtos_produto_id] FOREIGN KEY ([produto_id]) REFERENCES [Produtos] ([id_produto]) ON DELETE NO ACTION
);

CREATE INDEX [IX_InsumoCompra_fornecedor_id] ON [InsumoCompra] ([fornecedor_id]);

CREATE INDEX [IX_InsumoCompra_insumo_id] ON [InsumoCompra] ([insumo_id]);

CREATE INDEX [IX_InsumoEstoque_fornecedor_id] ON [InsumoEstoque] ([fornecedor_id]);

CREATE INDEX [IX_InsumoEstoque_insumo_id] ON [InsumoEstoque] ([insumo_id]);

CREATE INDEX [IX_Plantio_insumo_id] ON [Plantio] ([insumo_id]);

CREATE INDEX [IX_Plantio_produto_id] ON [Plantio] ([produto_id]);

CREATE INDEX [IX_ProdutoEstoque_produto_id] ON [ProdutoEstoque] ([produto_id]);

CREATE INDEX [IX_Produtos_insumo_id] ON [Produtos] ([insumo_id]);

CREATE INDEX [IX_Vendas_cliente_id] ON [Vendas] ([cliente_id]);

CREATE INDEX [IX_Vendas_produto_id] ON [Vendas] ([produto_id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241124000941_InitialCreate', N'9.0.0');

COMMIT;
GO

