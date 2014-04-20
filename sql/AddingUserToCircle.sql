
/*Adding User to the Circle*/
CREATE PROCEDURE AddUserToCircle
	@CircleId  INTEGER,  /*Id of the Circle to which the user is added*/
    @CustomerId INTEGER   /*Id of the User to be added to the Circle */
	
AS 

INSERT INTO [dbo].[CustomerBelongsToCircle] (CircleId,CustomerId, UserAccepted, OwnerAccepted)
   VALUES(@CircleId, @CustomerId, 0, 1)

GO