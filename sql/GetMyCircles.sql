
CREATE PROCEDURE GetMyCircles
    @UserId INTEGER
AS 

SELECT * FROM Circle as C
WHERE C.OwnerId = @UserId
GO