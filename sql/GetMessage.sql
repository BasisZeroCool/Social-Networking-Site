
CREATE PROCEDURE GetMessage
    @UserId INTEGER,
	@Id INTEGER
AS 

/* Only see messages that were sent to user or that user sent */
SELECT TOP 1 * FROM PrivateMessage as M
WHERE M.Id = @Id AND (M.RecieverId = @UserId OR M.SenderId = @UserId)

GO