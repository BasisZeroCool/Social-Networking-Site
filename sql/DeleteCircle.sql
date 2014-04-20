
CREATE PROCEDURE DeleteCircle
	@CircleId INTEGER,
	@UserId INTEGER

AS 

Delete From Circle
WHERE Id = @CircleId AND OwnerId = @UserId

GO