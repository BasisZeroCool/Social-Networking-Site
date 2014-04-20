
CREATE PROCEDURE RenameCircle
	@CircleId INTEGER,
	@CircleName char(50),
	@CircleType char(50),
	@UserId INTEGER

AS 

Update Circle
Set CircleName = @CircleName, CircleType = @CircleType
WHERE Id = @CircleId AND OwnerId = @UserId

GO