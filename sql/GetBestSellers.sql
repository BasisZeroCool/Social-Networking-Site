
CREATE PROCEDURE GetBestSellers
AS 

SELECT Top 10 * FROM Advertisement as A
WHERE A.NumUnits > A.NumSold /* only show ads with availiable units */
Order By A.NumSold DESC
GO