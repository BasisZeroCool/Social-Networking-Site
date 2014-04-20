
CREATE PROCEDURE UnlikeComment
	@CommentId INTEGER,
	@UserId INTEGER

AS 

DELETE FROM LikesComment
WHERE CommentId = @CommentId AND CustomerId = @UserId

GO