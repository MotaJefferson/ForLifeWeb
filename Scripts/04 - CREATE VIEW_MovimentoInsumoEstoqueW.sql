CREATE VIEW MovimentoInsumoEstoqueW
AS
SELECT idMovimentoProduto = id_estoque, 
	   nome = i.nome + ' - ' + i.tipo,
	   descrMovimento = CASE WHEN ie.tipo_movimento = 'E' THEN '[+] - Entrada' ELSE '[-] - Saída' END,
	   ie.quantidade_anterior, 
	   quantidadeMovimento =  CASE WHEN ie.tipo_movimento = 'E' THEN quantidade_entrada ELSE (quantidade_saida)*-1 END,
	   quantidade_atual,
	   data_registro
FROM InsumoEstoque ie
	 Join Insumos i ON i.id_insumo = ie.insumo_id