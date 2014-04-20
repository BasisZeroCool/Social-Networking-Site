
CREATE PROCEDURE AcceptJoin
    @UserId INTEGER,
	@CircleId INTEGER	
AS 
	Update CustomerBelongsToCircle
	Set OwnerAccepted = 1
	WHERE CustomerBelongsToCircle.CircleId = @CircleId AND CustomerBelongsToCircle.CustomerId = @UserId
GO