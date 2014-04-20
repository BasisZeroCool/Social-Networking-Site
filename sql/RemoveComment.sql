
CREATE PROCEDURE RemoveComment
	@CommentId INTEGER,
	@UserId INTEGER

AS 


IF (EXISTS(SELECT * FROM Comment AS C Where C.Id = @CommentId AND C.AuthorId = @UserId) /* Allow users to delete their own comments*/
OR EXISTS(
	SELECT * FROM Circle, [Page], Post, Comment
	WHERE Comment.PostId = Post.Id AND Post.PageId = [Page].Id AND [Page].CircleId = Circle.Id
	AND Comment.Id = @CommentId
	AND Circle.OwnerId = @UserId)) /* Allow users to delete comments on pages whose circles they own */
BEGIN

DELETE FROM LikesComment
WHERE CommentId = @CommentId

DELETE Comment
WHERE Id = @CommentId

RETURN(0)
END
ELSE
RETURN(1)
GO
