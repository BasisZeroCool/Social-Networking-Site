
CREATE PROCEDURE GetAccountHistory
    @Id INTEGER
AS 

SELECT S.DateOfSale, Ad.Company, Ad.ItemName, S.NumUnits, Ad.UnitPrice FROM Sale as S, Account as A, Advertisement as Ad
WHERE S.AccountNumber = A.AccountNumber AND A.AccountNumber = @Id AND Ad.Id = S.AdId

GO