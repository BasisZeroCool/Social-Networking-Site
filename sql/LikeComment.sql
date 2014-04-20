
/*Allows the User to Like a Post*/
CREATE PROCEDURE LikeComment
	@UserId  INTEGER,  /*Id of the user who likes the post*/
	@CommentId    INTEGER   /*Id of the Comment liked by the */
AS 

INSERT [dbo].[LikesComment] (CommentId,CustomerId)
   VALUES(@CommentId, @UserId)

GO