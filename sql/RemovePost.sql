
CREATE PROCEDURE RemovePost
	@PostId INTEGER,
	@UserId INTEGER

AS 


IF (EXISTS(SELECT * FROM Post AS P Where P.Id = @PostId AND P.AuthorId = @UserId) /* Allow users to delete their own posts*/
OR EXISTS(
	SELECT * FROM Circle, [Page], Post
	WHERE Post.PageId = [Page].Id AND [Page].CircleId = Circle.Id
	AND Post.Id = @PostId
	AND Circle.OwnerId = @UserId)) /* Allow users to delete posts on pages whose circles they own */
BEGIN

DELETE FROM Post Where Post.Id = @PostId

RETURN(0)
END
ELSE
RETURN(1)
GO