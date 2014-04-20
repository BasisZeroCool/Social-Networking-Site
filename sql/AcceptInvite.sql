
CREATE PROCEDURE AcceptInvite
    @UserId INTEGER,
	@CircleId INTEGER	
AS 
	Update CustomerBelongsToCircle
	Set UserAccepted = 1
	WHERE CustomerBelongsToCircle.CircleId = @CircleId AND CustomerBelongsToCircle.CustomerId = @UserId
GO