
CREATE PROCEDURE CreateACircle
    @CircleName CHAR(50), /*Name of the circle to be created*/
	@CircleType CHAR(50), /*Type of the circle to be created*/
	@OwnerId    INTEGER   /*User whose creating the circle*/

AS 

INSERT INTO Circle (CircleName, CircleType, OwnerId)
   VALUES(@CircleName, @CircleType, @OwnerId)        /*Creating a new circle with name, type and the id of the user*/

GO


