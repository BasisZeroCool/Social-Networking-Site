
CREATE PROCEDURE UnjoinCircle
    @UserId INTEGER,
	@CircleId INTEGER	
AS 
	DELETE FROM CustomerBelongsToCircle 
	Where CircleId = @CircleId AND CustomerId = @UserId

GO