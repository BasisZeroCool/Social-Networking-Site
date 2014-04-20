
CREATE PROCEDURE DeleteMessage
    @UserId INTEGER,
	@Id INTEGER
AS 

/* Only delete messages that were sent to user or that user sent */
DELETE FROM PrivateMessage
WHERE(PrivateMessage.RecieverId = @UserId OR PrivateMessage.SenderId = @UserId) AND PrivateMessage.Id = @Id
GO