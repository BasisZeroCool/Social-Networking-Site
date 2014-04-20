
/* Allows a Customer Representative to create an advertisment*/
CREATE PROCEDURE CreateAd
    @AdType char(50), /* The type of the advertisment (insurance, cars, etc)*/
    @Company varchar(MAX), /* The name of the company posting the ad*/
	@Content varchar(MAX), /* The description of the product, sale, etc*/
	@CreatorId char(64), /* The Id of the customer representative */
	@ItemName char(256), /* The name of the item being sold */
	@NumUnits INT, /* The number of units available for sale */
	@UnitPrice INT /* The price of each unit of item*/
AS 

INSERT INTO Advertisement (AdType, Company, Content, CreatorId, DateAdded, ItemName, NumUnits, UnitPrice)
Values (@AdType, @Company, @Content, @CreatorId,  GETDATE(), @ItemName, @NumUnits, @UnitPrice)
GO



