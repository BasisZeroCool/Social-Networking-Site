
/*Allows the User to Like a Post*/
CREATE PROCEDURE LikeAPost
	@UserId  INTEGER,  /*Id of the user who likes the post*/
	@PostId    INTEGER   /*Id of the Post liked by the */
AS 

INSERT [dbo].[LikesPost] (PostId,CustomerId)
   VALUES(@PostId, @UserId)

GO