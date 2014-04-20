
CREATE PROCEDURE EditPost
	@PostId INTEGER,
	@NewContent varchar(MAX),
	@UserId INTEGER

AS 


IF (EXISTS(SELECT * FROM Post AS P Where P.Id = @PostId AND P.AuthorId = @UserId) /* Allow users to edit their own posts*/
OR EXISTS(
	SELECT * FROM Circle, [Page], Post
	WHERE Post.PageId = [Page].Id AND [Page].CircleId = Circle.Id
	AND Post.Id = @PostId
	AND Circle.OwnerId = @UserId)) /* Allow users to edit comments on pages whose circles they own */
BEGIN

Update Post
Set Content = @NewContent
WHERE Id = @PostId

RETURN(0)
END
ELSE
RETURN(1)
GO