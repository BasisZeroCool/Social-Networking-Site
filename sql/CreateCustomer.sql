CREATE PROCEDURE CreateCustomer
    @Id INT,
	@Preferences varchar(MAX),
	@Rating int
AS 

INSERT INTO Customer (Id, Preferences, Rating)
Values (@Id, @Preferences, @Rating)

GO
