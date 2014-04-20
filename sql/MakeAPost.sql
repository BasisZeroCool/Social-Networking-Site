
/*To make a Post*/
CREATE PROCEDURE MakeAPost
	@Content   VARCHAR(MAX), /*The content to be posted*/
	@AuthorId  INTEGER,     /*Id of the User creating the post*/
	@PageId    INTEGER      /*Id of the Page where the Post is made*/

AS 

INSERT [dbo].[Post] (DateAdded, Content, AuthorId, PageId)
   VALUES(GETDATE(), @Content, @AuthorId, @PageId)

GO