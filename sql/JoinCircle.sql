
CREATE PROCEDURE JoinCircle
    @UserId INTEGER,
	@CircleId INTEGER	
AS 
	INSERT INTO CustomerBelongsToCircle(CircleId, CustomerId, UserAccepted, OwnerAccepted)
	VALUES(@CircleId, @UserId, 1, 0)
GO