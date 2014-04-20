
CREATE PROCEDURE SendMessage
    @Sender INTEGER,
	@Reciever INTEGER,
	@Subject CHAR(256),
	@Content VARCHAR(MAX)
	
AS 

IF (SELECT Count(*) FROM Customer as C
WHERE C.Id = @Sender OR C.Id = @Reciever) = 2
BEGIN

INSERT INTO PrivateMessage (SenderId, RecieverId, MessageSubject, Content, DateAdded)
Values (@Sender, @Reciever, @Subject, @Content, GETDATE())

RETURN(0)
END
ELSE
RETURN(1)
GO