
CREATE PROCEDURE GetCircles
    @UserId INTEGER
AS 

SELECT * FROM Circle as C
WHERE C.OwnerId = @UserId OR EXISTS(SELECT * FROM CustomerBelongsToCircle as B
 WHERE B.CircleId = C.Id AND B.CustomerId = @UserId)
GO