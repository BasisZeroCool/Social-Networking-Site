
CREATE PROCEDURE MailingList
AS 

Select U.FirstName, U.LastName, U.EmailAddress From [User] as U
WHERE EXISTS(SELECT * FROM Customer as C WHERE U.Id = C.Id)

GO