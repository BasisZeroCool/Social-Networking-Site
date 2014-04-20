
CREATE PROCEDURE RecordTransaction
    @NumUnits INT,
	@AdId INT,
	@AccountNumber INT
AS 


IF (SELECT a.NumUnits FROM Advertisement as A
Where A.Id = @AdId) >= (SELECT a.NumSold FROM Advertisement as A
Where A.Id = @AdId) + @NumUnits
BEGIN
	INSERT INTO Sale(DateOfSale, NumUnits, AdId, AccountNumber)
	VALUES(GETDATE(), @NumUnits, @AdId, @AccountNumber)

	UPDATE Advertisement
	SET NumSold = NumSold + @NumUnits
	WHERE Id = @AdId

	RETURN(0)
END
ELSE
	RETURN(1)
GO