/****** Script for SelectTopNRows command from SSMS  ****/
CREATE PROCEDURE GetUserAccounts
AS 
Select A.AccountNumber, A.CreditCardNumber, U.EmailAddress
From Customer as C, Account as A, [User] as U
Where A.CustomerId = C.Id and U.Id = C.Id