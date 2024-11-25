CREATE VIEW RelatorioVendasW
AS
SELECT 
	v.numero_venda,
    v.data_venda,
    v.quantidade_venda,
    v.valor_venda,
    v.preco_unitario,
    p.nome AS nome_produto,
    c.nome AS nome_cliente,
    c.CPF AS cpf_cliente
FROM Vendas v
	JOIN Produtos p ON v.produto_id = p.id_produto
	JOIN Clientes c ON v.cliente_id = c.id_cliente
GO
-----------------------------------------------------------------------------------

  CREATE VIEW RelatorioCompraInsumoW
AS
 Select
		 i.nome AS nomeInsumo,
		 f.CNPJ, 
		 f.nome AS Fornecedor, 
		 ic.quantidade, 
		 ic.preco_unitario, 
		 ic.valor_compra
 From InsumoCompra ic
		Join Insumos i ON i.id_insumo = ic.insumo_id
		Join Fornecedores f ON f.id_fornecedor = ic.fornecedor_id
GO
----------------------------------------------------------------------------------------

CREATE VIEW RelatorioControleEstoqueInsumoW
AS
Select  i.nome AS nomeInsumo,
		i.tipo,
		ie.quantidade_entrada,
		ie.quantidade_saida,
		ie.quantidade_atual,
		f.nome AS Nome_Fornecedor
From InsumoEstoque ie
		Join InsumoCompra ic ON ie.insumo_id = ic.insumo_id
		Join Insumos i ON i.id_insumo = ie.insumo_id
		Join Fornecedores f ON f.id_fornecedor = ie.fornecedor_id
GO		
----------------------------------------------------------------------------------------

CREATE VIEW RelatorioControleEstoqueProdutoW
AS
Select 
		p.nome AS nomeProduto,
		pe.data_colheita,
		pe.quantidade_saida,
		pe.data_saida,
		pe.quantidade_atual,
		p.periodo_vencimento	
From Produtos p
		Join ProdutoEstoque pe ON p.id_produto = pe.produto_id
GO		

----------------------------------------------------------------------------------------

CREATE VIEW TelaEstoqueInsumoW
 AS
 SELECT I.nome
      ,E.quantidade_atual
	  ,E.data_vencimento_estimado
  FROM Insumos I
  JOIN InsumoEstoque E ON E.insumo_id = I.id_insumo
  GO

----------------------------------------------------------------------------------------

CREATE VIEW TelaEstoqueProdutoW
 AS
 SELECT P.nome
      ,E.quantidade_atual
	  ,E.data_vencimento_estimado
  FROM Produtos P
  JOIN ProdutoEstoque E ON E.produto_id = P.id_produto
  GO
