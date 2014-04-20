USE SQLException

/*USER TABLE*/
INSERT [dbo].[User] (Id, FirstName, LastName, [Address], City, [State], ZipCode, Telephone, EmailAddress, DateAdded, DOB , Gender )
   VALUES (100100101,'Alice','McKeeny', 'Chapin Apt 2010,Health Drive','Stony Brook','NY','11790','4314649881', 'alice@blah.com' , GETDATE(), '19881010', 'F'),
          (100100102,'Bob','Wanderwall', '21 MajorApt,Oak St.','New York','NY','11700','4314649882', 'blob@blah.com' , GETDATE(), '19880608', 'M'),
		  (100100103,'Elisa','Roth', '43 Corvette Apt, Maple St','Stony Brook','NY','11790','4314649883', 'elisa@blah.com' , GETDATE(), '19921110', 'F'),
		  (100100104,'Kelly','McDonald', '54 East Apt,Oak St','New York','NY','11700','4314649883', 'kelly@blah.com' , GETDATE(), '19911111', 'F'),
		  (100100105,'Wendy','Stanley', '21 MajorApt,Oak St.','Stony Brook','NY','11790','4314649884', 'wendy@blah.com' , GETDATE(), '19920808', 'F'),
		  (100100106,'Dennis','Ritchie', '43 Corvette Apt, Maple St','New York','NY','11700','4314649885', 'den@blah.com' , GETDATE(), '19920302', 'M'),
		  (100100107,'Patrick','Norris', 'Chapin Apt 1001,Health Drive','Stony Brook','NY','11790','4314649886', 'patnor@blahblah.com' , GETDATE(), '19920807', 'M'),
		  (100100108,'Chuck','Stewart', '54 East Apt,Oak St','New York','NY','11700','4314649887', 'chuck@blah.com' , GETDATE(), '19910201', 'M'),
		  (100100109,'Brad','Norton', 'Chapin Apt 2010,Health Drive','Stony Brook','NY','11790','4314649888', 'brad@blah.com' , GETDATE(), '19920901', 'M'),
		  (100100110,'Jennifer','Buffet', 'Chapin Apt 1223,Health Drive','New York','NY','11700','4314649889', 'jennycool123@blah.com' , GETDATE(), '19890801', 'F'),
		  (111221, 'Mike','Thomas','43 Apple Apt,Maple Street', 'Stony Brook','NY','11790','6314648998', 'Mike.Thomas@sqlexception.com', '20110410', NULL ,'M'),
		  (111222, 'Jonthan','Klaus','76 PotterApt,Muriel Avenue', 'Stony Brook','NY','11790','6314651232', 'Jonathan.Klaus@sqlexception.com', '20110505', NULL ,'M'),
		  (111220, 'Scott','Thomas','11 Oak St,Mart Avenue', 'Stony Brook','NY','11790','4312345432','Scott.Thomas@sqlexception.com', '20110105', NULL ,'M')	 
GO

/*CUSTOMER TABLE*/
INSERT [dbo].[Customer] (Id, Preferences, Rating)
	VALUES(100100101, 'cars,life insurance', 8),
	      (100100102, 'cars,clothing', 5),
		  (100100103, 'clothing', 5),
		  (100100104, 'clothing,toys', 5),
		  (100100105, 'life insurance', 2),
		  (100100106, 'life insurance', 2),
		  (100100107, 'cars,clothing', 2),
		  (100100108, 'clothing,life insurance', 2),
		  (100100109, 'life insurance', 2),
		  (100100110, 'life insurance', 2)
GO

/*EMPLOYEE TABLE*/
INSERT [dbo].[Employee] (Id, SSN, StartDate, HourlyRate)
    VALUES (111221, 111222333, '20110410', 20),
	       (111222, 111333222, '20110505', 20),
		   (111220, 111444111, '20110105', 35)
GO

/*CUSTOMER REP TABLE*/
INSERT [dbo].[CustomerRep] (Id, IsSupervisedBy)
    VALUES(111221, NULL),
	      (111222, NULL)
GO

/*SITE MANAGER TABLE*/
INSERT [dbo].[SiteManager] (Id)
    VALUES(111220)
GO

/*ACCOUNT TABLE*/
INSERT [dbo].[Account] (AccountNumber, CreditCardNumber, CustomerId, DateAdded )
	VALUES (90010101, '4123132454456550', 100100101, '20110410'),
	       (90010102, '1221344356657880', 100100102, '20110410'),
	       (90010103, '9889677645543220', 100100103, '20110410'),
           (90010104, '1221655609907660', 100100104, '20110410'),
	       (90010105, '1221322334434550', 100100105, '20110510'),
		   (90010106, '9889877867764550', 100100106, '20110510'),
		   (90010107, '3443566576678770', 100100107, '20110610'),
		   (90010108, '1221122132232330', 100100108, '20110610'),
		   (90010109, '1234432145544550', 100100109, '20110610'),
		   (90010110, '2345543289000980', 100100110, '20110610'),
		   (90010111, '2345543282424980', 100100101, '20110710'),
		   (90010112, '2345543289003440', 100100102, '20110710')
		   
GO

/*CIRCLE TABLE*/
INSERT [dbo].[Circle] (Id, CircleName, CircleType, OwnerId)
     VALUES(8001, 'My Friends', 'Friends', 100100101),
	       (8002, 'Best Friends', 'Friends', 100100102),
		   (8003, 'StonyBrookGang', 'Friends', 100100105),
		   (8004, 'CSJunkies', 'Group', 100100107),
		   (8005, 'Norris Family', 'Family', 100100109),
           (8006, 'Microsoft Groupies', 'Company', 100100106)
GO

/*PAGE TABLE*/
INSERT [dbo].[Page] (Id, PostCount, CircleId )
	 VALUES (6900, 2, 8001),
	        (6902, 1, 8002), /* Added since PageId is needed in Circle Table*/
	        (6904, 1, 8003),
			(6905, 1, 8004),
			(6908, 0, 8005),
			(6910, 1, 8006)
GO 	

/*POST TABLE*/	
INSERT [dbo].[Post] (Id, DateAdded, Content, CommentCount, AuthorId, PageId )	
    VALUES (20111, '20111010', 'Its Snowing! :D', 2, 100100105, 6904),
	       (20112, '20111110', 'GO Seawolves!!!!', 3, 100100106, 6910),
		   (20113, '20111110', 'Arrgh!I hate facebook!', 0, 100100103, 6900),
		   (20114, '20111210', 'MackBook Finally!!!', 1, 100100104, 6900),
		   (20115, '20111210', 'ritchie RIP :(', 0, 100100104, 6905)
GO		

/*COMMENT TABLE*/
INSERT [dbo].[Comment] (Id, Content, DateAdded, AuthorId, PostId )	
    VALUES (900001, 'Its beautiful! :)', '20111010', 100100101, 20111),
	       (900002, 'Nature''s white blanket :D', '20111110', 100100107, 20111),
		   (900003, 'GO! GO! GO!', '20111110', 100100104, 20112),
		   (900004, 'we totally owned them!', '20111110', 100100103, 20112),
		   (900005, 'we won! We won!', '20111210', 100100102, 20112),
		   (900006, 'Congrats!', '20111210', 100100109, 20114)
GO

/*PRIVATE MESSAGE TABLE*/
INSERT [dbo].[PrivateMessage] (Id, DateAdded, MessageSubject, Content, SenderId, RecieverId)
    VALUES (3001, '20111010', 'hey!', 'Hey! Do u have assignent 1 questions?', 100100101, 100100102),
		   (3002, '20111010', 're: hey!', 'nope? I think patrick has them.', 100100102, 100100101),
		   (3003, '20111111', 'happy bday!', 'hey u there! Have an amazing and super duper bday! Don?t miss me too much :D', 100100103, 100100104),
		   (3004, '20111110', 'will be late', 'Hey! I am sorry I wont make it to tonights appointment.Stuck with some work! :(', 100100105, 100100105)
GO

/*ADVERTISEMENT TABLE*/	
INSERT [dbo].[Advertisement] (Id, Company, NumUnits, NumSold, ItemName, Content, DateAdded, UnitPrice, AdType, CreatorId)
    VALUES (33331, 'Ford', 30, 0, '2012-Mustang', 'Ford Mustang! First 10 cutomers get a 10%Discount!', '20110410', 22000, 'car', 111221),
	       (33332, 'GAP', 100 , 0, 'Superman Shirt', 'Just $5!!!!!!!', '20110410', 5, 'clothing', 111222)
GO

/*SALE TABLE*/
INSERT [dbo].[Sale] (TransactionId, DateOfSale, NumUnits, AdId, AccountNumber)
    VALUES (200010001, '20110422', 1, 33331, 90010101),
	       (200010002, '20110423', 1, 33332, 90010101),
		   (200010003, '20110423', 1, 33332, 90010102),
		   (200010004, '20110423', 1, 33332, 90010103)
GO



/*CUSTOMER BELONGS TO CIRCLE*/
INSERT [dbo].[CustomerBelongsToCircle] (CircleId, CustomerId, UserAccepted, OwnerAccepted)
    VALUES (8001, 100100101, 1, 1),
		   (8002, 100100102, 1, 1),
		   (8003, 100100105, 1, 1),
		   (8004, 100100107, 1, 1),
		   (8005, 100100109, 1, 1),
		   (8006, 100100106, 1, 1),
		   (8001, 100100102, 1, 1),
		   (8001, 100100103, 1, 1),
		   (8001, 100100104, 1, 1),
		   (8002, 100100101, 1, 1),
		   (8002, 100100110, 1, 1),
		   (8003, 100100106, 1, 1),
		   (8004, 100100103, 1, 1),
		   (8004, 100100104, 1, 1),
		   (8005, 100100108, 1, 1),
		   (8005, 100100110, 1, 1),
		   (8005, 100100105, 1, 1),
		   (8006, 100100107, 1, 1),
		   (8006, 100100108, 1, 1),
		   (8006, 100100109, 1, 1)
GO

/*LIKES POST*/
INSERT [dbo].[LikesPost] (PostId, CustomerId)
    VALUES (20111, 100100101),
           (20111, 100100102),
		   (20111, 100100103),
		   (20111, 100100104),
		   (20112, 100100101),
		   (20112, 100100102),
		   (20112, 100100103),
		   (20112, 100100104),
		   (20112, 100100105),
		   (20112, 100100107),
		   (20112, 100100108),
		   (20112, 100100109),
		   (20113, 100100105),
		   (20114, 100100106),
		   (20114, 100100102)
GO

/*LIKES COMMENT*/
INSERT [dbo].[LikesComment] (CommentId, CustomerId)
    VALUES(900002, 100100101),
	      (900002, 100100102),
		  (900002, 100100103),
		  (900002, 100100104),
		  (900004, 100100106),
		  (900004, 100100107),
		  (900004, 100100108)
GO

