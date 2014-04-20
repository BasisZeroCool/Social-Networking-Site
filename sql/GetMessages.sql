
CREATE PROCEDURE GetMessages
    @UserId INTEGER
AS 

/* Only see messages that were sent to user or that user sent */
SELECT M.Id, M.DateAdded, M.MessageSubject, M.SenderId, M.RecieverId 
FROM PrivateMessage as M
WHERE(M.RecieverId = @UserId OR M.SenderId = @UserId)

GO