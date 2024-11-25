CREATE TRIGGER ImpedirExclusaoInsumo
ON Insumos
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Verifica se o insumo est� vinculado a outras tabelas
    IF EXISTS (
        SELECT 1
        FROM InsumoCompra
        WHERE insumo_id IN (SELECT id_insumo FROM deleted)
    )
    BEGIN
        RAISERROR ('Este insumo est� vinculado a compras e n�o pode ser exclu�do.', 16, 1)
        ROLLBACK TRANSACTION
        RETURN
    END

    IF EXISTS (
        SELECT 1
        FROM Produtos
        WHERE insumo_id IN (SELECT id_insumo FROM deleted)
    )
    BEGIN
        RAISERROR ('Este insumo est� vinculado a produtos e n�o pode ser exclu�do.', 16, 1)
        ROLLBACK TRANSACTION
        RETURN
    END

    -- Se n�o estiver vinculado a outras tabelas, procede com a exclus�o
    DELETE FROM Insumos WHERE id_insumo IN (SELECT id_insumo FROM deleted)
END
