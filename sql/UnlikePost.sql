
CREATE PROCEDURE UnlikePost
	@PostId INTEGER,
	@UserId INTEGER

AS 

DELETE FROM LikesPost
WHERE PostId = @PostId AND CustomerId = @UserId

GO