
CREATE PROCEDURE UpdateCustomerInfo
    @UserName INTEGER,
	@Preferences varchar(MAX),
	@FirstName char(32),
	@LastName char (32),
	@Address char (128),
	@City char(128),
	@State char(2),
	@ZipCode char(5),
	@Telephone char(10),
	@EmailAddress char(128),
	@Rating INTEGER
AS 

IF EXISTS(SELECT * FROM Customer as C
WHERE C.Id = @UserName)
BEGIN

UPDATE Customer
SET Preferences = @Preferences,
Rating = @Rating
WHERE Id = @UserName

UPDATE [User]
SET FirstName = @FirstName,
LastName = @LastName,
[Address] = @Address,
City = @City,
[State] = @State,
ZipCode = @ZipCode,
Telephone = @Telephone,
EmailAddress = @EmailAddress
WHERE Id = @UserName

RETURN(0)
END
ELSE
RETURN(1)
GO