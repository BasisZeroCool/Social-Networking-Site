
CREATE PROCEDURE GetRecomendations
    @CustId INTEGER /* The user for who to get recommendations */
AS 

SELECT Distinct Ad2.Id, Ad2.ItemName FROM Advertisement as Ad1, Advertisement as Ad2, Account as Ac, Sale as S
WHERE Ac.CustomerId = @CustId AND S.AccountNumber = Ac.AccountNumber /* all sales by users accounts */
AND S.AdId = Ad1.Id   /* All Ads associated with those sales */
AND Ad1.Id <> AD2.Id AND (Ad1.AdType = Ad2.AdType OR Ad1.Company = Ad2.Company OR Ad1.ItemName = Ad2.ItemName) /* Similar Ads, (but not the same) */
AND Ad2.NumUnits > Ad2.NumSold /* Only show ads with availiable units */
GO