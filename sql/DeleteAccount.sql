
CREATE PROCEDURE DeleteAccount 
	@Id INT
AS 

Delete From Account 
Where AccountNumber = @Id
GO
