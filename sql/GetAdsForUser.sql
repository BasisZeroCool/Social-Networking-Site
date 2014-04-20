
CREATE PROCEDURE GetAdsByType
@AdType char(50)
AS 

Select A.Id, A.ItemName, A.Company, A.NumUnits, A.NumSold, A.UnitPrice, A.Content From Advertisement as A
WHERE A.AdType = @AdType AND (A.NumUnits - A.NumSold) > 0

GO