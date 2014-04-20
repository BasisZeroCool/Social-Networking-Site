
CREATE PROCEDURE DeleteUser
    @Id INTEGER
AS 

/* Only delete messages that were sent to user or that user sent */
DELETE FROM [User]
WHERE Id = @Id
GO