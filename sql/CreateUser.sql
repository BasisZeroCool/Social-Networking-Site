CREATE PROCEDURE CreateUser
    @FirstName char(32),
	@LastName char(32),
	@Address char(128),
	@City char(128),
	@State char(2),
	@ZipCode char(5),
	@Telephone char(10),
	@EmailAddress char(128),
	@DOB datetime,
	@Gender char(1),
	@Password char(50)
AS 

INSERT INTO [User] (FirstName, LastName, Address, City, State, ZipCode, Telephone, EmailAddress, DateAdded, DOB, Gender, Password)
Values (@FirstName, @LastName, @Address, @City, @State, @ZipCode, @Telephone, @EmailAddress, GETDATE(), @DOB, @Gender, @Password)

SELECT SCOPE_IDENTITY();

GO
