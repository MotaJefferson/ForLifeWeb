CREATE TRIGGER ImpedirExclusaoInsumo
ON Insumos
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Verifica se o insumo está vinculado a outras tabelas
    IF EXISTS (
        SELECT 1
        FROM InsumoCompra
        WHERE insumo_id IN (SELECT id_insumo FROM deleted)
    )
    BEGIN
        RAISERROR ('Este insumo está vinculado a compras e não pode ser excluído.', 16, 1)
        ROLLBACK TRANSACTION
        RETURN
    END

    IF EXISTS (
        SELECT 1
        FROM Produtos
        WHERE insumo_id IN (SELECT id_insumo FROM deleted)
    )
    BEGIN
        RAISERROR ('Este insumo está vinculado a produtos e não pode ser excluído.', 16, 1)
        ROLLBACK TRANSACTION
        RETURN
    END

    -- Se não estiver vinculado a outras tabelas, procede com a exclusão
    DELETE FROM Insumos WHERE id_insumo IN (SELECT id_insumo FROM deleted)
END
