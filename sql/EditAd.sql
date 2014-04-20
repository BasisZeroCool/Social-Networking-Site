CREATE PROCEDURE EditAccount
    @CardNumber nvarchar(MAX), 
	@Id INT
AS 

Update [Account]
Set CreditCardNumber = @CardNumber
Where AccountNumber = @Id
GO


