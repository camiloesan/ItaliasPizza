CREATE FUNCTION dbo.fn_CalculateMaxProducts
(
    @IdProduct UNIQUEIDENTIFIER
)
RETURNS INT
AS
BEGIN
    DECLARE @MaxProducts INT;

    SELECT @MaxProducts = MIN(CAST(S.Quantity / RS.SupplyAmount AS INT))
    FROM Recipe R
    JOIN RecipeSupply RS ON R.IdRecipe = RS.IdRecipe
    JOIN Supply S ON RS.IdSupply = S.IdSupply
    WHERE R.IdProduct = @IdProduct;

    RETURN @MaxProducts;
END;
GO