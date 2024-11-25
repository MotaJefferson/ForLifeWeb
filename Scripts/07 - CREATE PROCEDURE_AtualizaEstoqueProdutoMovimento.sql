CREATE PROCEDURE AtualizaEstoqueProdutoMovimento (
	@TipoMovimento CHAR, 
	@quantidade INT,  
	@produto_id INT, 
	@data_colheita DATETIME, 
	@data_vencimento_estimado DATETIME, 
	@data_saida DATETIME)

AS


DECLARE @quantidadeAnterior INT , 
		@quantidadeAtual INT

SELECT @quantidadeAnterior = isnull((select quantidade_atual FROM ProdutoEstoque ant WHERE id_estoque = (SELECT MAX (id_estoque) FROM ProdutoEstoque ins WHERE produto_id = @produto_id)),0) 

IF (@TipoMovimento = 'E') /*Movimento de entrada*/

BEGIN 

	SET @quantidadeAtual = @quantidadeAnterior + @quantidade

 INSERT INTO ProdutoEstoque (produto_id, quantidade_anterior,quantidade_atual,tipo_movimento, quantidade_colheita, data_colheita, data_vencimento_estimado, data_registro )
	SELECT @produto_id,@quantidadeAnterior, @quantidadeAtual, UPPER (@TipoMovimento), @quantidade, @data_colheita, @data_vencimento_estimado, GETDATE ()

END
ELSE IF(@TipoMovimento = 'S') /* Movimento de Saída */
BEGIN 
	SET @quantidadeAtual = @quantidadeAnterior + (@quantidade)*-1
	
	IF (@quantidadeAtual < 0) /*Verificar se o estoque ficará negativo*/
	BEGIN 
		RAISERROR ('Não permitido! Essa movimentação deixará o estoque negativo.', 16, 1)
		RETURN;
	END
	ELSE
	BEGIN

	 INSERT INTO ProdutoEstoque (produto_id, quantidade_anterior,quantidade_atual,tipo_movimento, quantidade_saida, data_saida, data_registro )
	SELECT @produto_id,@quantidadeAnterior, @quantidadeAtual, UPPER (@TipoMovimento), @quantidade, @data_saida,  GETDATE ()

	END
END
ELSE
BEGIN 
RAISERROR ('Tipo de movimento inválido. São permitidos movimentos: [E - Entrada e S - Saída]', 16, 1)
		RETURN;
END