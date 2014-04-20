
CREATE View CustomerAcceptedIntoCircle

AS 

Select * From CustomerBelongsToCircle AS CBC
Where CBC.OwnerAccepted = 1 AND CBC.UserAccepted = 1
GO
