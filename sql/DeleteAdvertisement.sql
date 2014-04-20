
CREATE PROCEDURE DeleteAd
    @Id INT
AS 
/* This will fail if there are sales associated with this advertisement */
DELETE FROM Advertisement
WHERE Advertisement.Id = @Id 
GO