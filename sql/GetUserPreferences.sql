
CREATE PROCEDURE GetPreferences
    @CustId INTEGER /* The user for who to get recommendations */
AS 
SELECT Top 1 C.Preferences
  FROM Customer as C
  Where C.Id = @CustId;
