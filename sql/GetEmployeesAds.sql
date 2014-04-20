/****** Script for SelectTopNRows command from SSMS  ****/
CREATE PROCEDURE GetEmployeesAds
    @email NVARCHAR(MAX)
AS 
Select * From Advertisement as A, CustomerRep as C, [User] as U
Where A.CreatorId = C.Id AND U.Id = C.Id AND U.EmailAddress = @email
