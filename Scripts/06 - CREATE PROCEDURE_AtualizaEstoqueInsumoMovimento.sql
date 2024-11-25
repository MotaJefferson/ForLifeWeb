CREATE PROCEDURE AtualizaEstoqueInsumoMovimento (
	@TipoMovimento CHAR, 
	@quantidade INT,  
	@insumo_id INT, 
	@fornecedor_id INT,
	@data_Entrada DATETIME, 
	@data_vencimento_estimado DATETIME, 
	@data_saida DATETIME) 

AS

DECLARE @quantidadeAnterior INT , 
		@quantidadeAtual INT

SELECT @quantidadeAnterior = isnull((select quantidade_atual FROM InsumoEstoque ant WHERE id_estoque = (SELECT MAX (id_estoque) FROM InsumoEstoque ins WHERE insumo_id = @insumo_id)),0) 

IF (@TipoMovimento = 'E') /*Movimento de entrada*/

BEGIN 

	SET @quantidadeAtual = @quantidadeAnterior + @quantidade

 INSERT INTO InsumoEstoque (insumo_id, fornecedor_id, quantidade_anterior, quantidade_entrada, quantidade_atual, tipo_movimento, data_registro, data_entrada, data_vencimento_estimado)
	select insumo_id = @insumo_id, fornecedor_id = @fornecedor_id, quantidade_anterior = @quantidadeAnterior, quantidade_entrada = @quantidade, quantidade_atual = @quantidadeAtual, tipoMovimento = UPPER(@TipoMovimento),
	       dataMovimento = GETDATE(), 	data_entrada = @data_Entrada, data_vencimento_estimado = @data_vencimento_estimado

END
ELSE IF(@TipoMovimento = 'S') /* Movimento de Saída */
BEGIN 
	SET @quantidadeAtual = @quantidadeAnterior + (@quantidade)*-1
	
	IF (@quantidadeAtual < 0) /*Verificar se o estoque ficará negativo*/
	BEGIN 
		RAISERROR ('Não permitido! Essa movimentação deixará o estoque negativo.', 16, 1)
		RETURN;
	END
	else
	begin

	INSERT INTO InsumoEstoque (insumo_id, fornecedor_id, quantidade_anterior, quantidade_saida, quantidade_atual, tipo_movimento, data_registro, data_saida)
	select insumo_id = @insumo_id, fornecedor_id = @fornecedor_id, quantidade_anterior = @quantidadeAnterior, quantidade_saida = @quantidade, quantidade_atual = @quantidadeAtual, tipoMovimento = UPPER(@TipoMovimento),
	       dataMovimento = GETDATE(), 	data_saida = @data_saida
	end
END
ELSE
BEGIN 
RAISERROR ('Tipo de movimento inválido. São permitidos movimentos: [E - Entrada e S - Saída]', 16, 1)
		RETURN;
END

