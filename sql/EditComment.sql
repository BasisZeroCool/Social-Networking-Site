
CREATE PROCEDURE EditComment
	@CommentId INTEGER,
	@NewContent varchar(MAX),
	@UserId INTEGER

AS 


IF (EXISTS(SELECT * FROM Comment AS C Where C.Id = @CommentId AND C.AuthorId = @UserId) /* Allow users to edit their own comments*/
OR EXISTS(
	SELECT * FROM Circle, [Page], Post, Comment
	WHERE Comment.PostId = Post.Id AND Post.PageId = [Page].Id AND [Page].CircleId = Circle.Id
	AND Comment.Id = @CommentId
	AND Circle.OwnerId = @UserId)) /* Allow users to edit comments on pages whose circles they own */
BEGIN

Update Comment
Set Content = @NewContent
WHERE Id = @CommentId

RETURN(0)
END
ELSE
RETURN(1)
GO
