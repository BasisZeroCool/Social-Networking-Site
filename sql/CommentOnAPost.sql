
/*Allows the user to comment on a Post*/
CREATE PROCEDURE CommentOnAPost
	@Content   VARCHAR(MAX),  /*Content of the comment made by the user*/
	@AuthorId  INTEGER,   /*Id of the user who made the post*/
	@PostId    INTEGER    /*Id of the Post on which a comment is made*/

AS 
IF EXISTS (SELECT * FROM dbo.Circle C, dbo.CustomerBelongsToCircle CBC, dbo.Page P, dbo.Post PT
           WHERE P.CircleId = C.Id AND CBC.CircleId = C.Id AND CBC.CustomerId = @AuthorId AND PT.PageId = P.Id AND PT.Id = @PostId AND CBC.OwnerAccepted = 1 AND CBC.UserAccepted = 1)
BEGIN 
INSERT [dbo].[Comment] (DateAdded, Content, AuthorId, PostId)
   VALUES(GETDATE(), @Content, @AuthorId, @PostId)

END

GO