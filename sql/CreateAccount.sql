
CREATE PROCEDURE CreateAccount
@CardNumber nvarchar(MAX),
	@DateAdded DATETIME, 
	@CustId    INTEGER   

AS 

INSERT INTO Account (CreditCardNumber, DateAdded, CustomerId)
   VALUES(@CardNumber, @DateAdded, @CustId)     

GO


