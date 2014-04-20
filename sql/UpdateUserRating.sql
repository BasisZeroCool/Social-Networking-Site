
CREATE PROCEDURE UpdateCustomerInfo
    @UserName INTEGER,
	@Rating decimal
AS 

IF EXISTS(SELECT * FROM Customer as C
WHERE C.Id = @UserName)
BEGIN

UPDATE Customer
SET Rating = @Rating
WHERE Id = @UserName

RETURN(0)
END
ELSE
RETURN(1)
GO