
CREATE PROCEDURE RemoveUserFromCircle
	@OwnerId INTEGER,
	@CircleId INTEGER,
	@UserId INTEGER

AS 


IF (EXISTS(Select * from Circle Where Id = @CircleId AND OwnerId = @OwnerId)) /* must be the owner */

BEGIN

DELETE FROM CustomerBelongsToCircle Where CircleId = @CircleId AND CustomerId = @UserId

RETURN(0)
END
ELSE
RETURN(1)
GO