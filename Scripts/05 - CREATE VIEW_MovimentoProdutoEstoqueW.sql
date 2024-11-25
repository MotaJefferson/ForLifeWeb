CREATE VIEW MovimentoProdutoEstoqueW
AS
SELECT idMovimentoProduto = id_estoque, 
	   p.nome,
	   descrMovimento = CASE WHEN pe.tipo_movimento = 'E' THEN '[+] - Entrada' ELSE '[-] - Saída' END,
	   pe.quantidade_anterior, 
	   quantidadeMovimento =  CASE WHEN pe.tipo_movimento = 'E' THEN quantidade_colheita ELSE (quantidade_saida)*-1 END,
	   quantidade_atual,
	   data_registro
FROM ProdutoEstoque pe
	 Join Produtos p ON p.id_produto = pe.produto_id

	