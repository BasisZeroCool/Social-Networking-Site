CREATE DATABASE SQLException
GO

USE SQLException;
GO

CREATE TABLE Circle(
	Id INTEGER IDENTITY(1,1),
	CircleName CHAR(50) NOT NULL,
	CircleType CHAR(50),
	OwnerId INTEGER NOT NULL,
	PRIMARY KEY(Id),
	CHECK(Id > 0),
	UNIQUE(CircleName, OwnerId) /* Users cannot create multiple circles with the same name */
)

CREATE TABLE [Page](
	Id INTEGER IDENTITY(1,1),
	PostCount INTEGER DEFAULT 0,
	CircleId INTEGER NOT NULL,
	PRIMARY KEY(Id),
	CHECK(Id > 0)
)

CREATE TABLE Post(
	Id INTEGER IDENTITY(1,1),
	DateAdded DATETIME NOT NULL,
	Content VARCHAR(MAX) NOT NULL,
	CommentCount INTEGER DEFAULT 0,
	AuthorId INTEGER NOT NULL, 
	PageId INTEGER NOT NULL,
	PRIMARY KEY(Id),
	CHECK(Id > 0)
)

CREATE TABLE Comment(
	Id INTEGER IDENTITY(1,1),
	Content VARCHAR(Max) NOT NULL,
	DateAdded DATETIME NOT NULL,
	AuthorId INTEGER NOT NULL,
	PostId INTEGER NOT NULL,
	PRIMARY KEY(Id),
	CHECK(Id > 0)
)

CREATE TABLE PrivateMessage(
	Id INTEGER IDENTITY(1,1),
	DateAdded DATETIME NOT NULL,
	MessageSubject CHAR(256),
	Content VARCHAR(MAX) NOT NULL,
	SenderId INTEGER NOT NULL,
	RecieverId INTEGER NOT NULL,
	PRIMARY KEY(Id),
	CHECK(Id > 0)
)

CREATE TABLE Advertisement(
	Id INTEGER  IDENTITY(1,1),
	Company VARCHAR(MAX) NOT NULL,
	NumUnits INTEGER NOT NULL,
	NumSold INTEGER NOT NULL DEFAULT 0,
	ItemName CHAR(256) NOT NULL,
	Content VARCHAR(MAX),
	DateAdded DATETIME NOT NULL,
	UnitPrice DECIMAL NOT NULL,
	AdType CHAR(50) NOT NULL,
	CreatorId INTEGER NOT NULL,
	PRIMARY KEY(Id),
	CHECK (NumUnits > 0),
	CHECK (UnitPrice > 0),
	CHECK(Id > 0)
)

CREATE TABLE [User](
	Id INT  IDENTITY(1,1),
	FirstName CHAR(32) NOT NULL,
	LastName CHAR(32) NOT NULL,
	[Address] CHAR(128) NOT NULL,
	City CHAR(128) NOT NULL,
	[State] CHAR(2) NOT NULL,
	ZipCode CHAR(5) NOT NULL,
	Telephone CHAR(10) NOT NULL,
	EmailAddress CHAR(128) NOT NULL,
	DateAdded DATETIME NOT NULL,
	DOB DATETIME,
	Gender CHAR(1),
	Password CHAR(50),
	PRIMARY KEY(Id),
	UNIQUE(EmailAddress)
)

CREATE TABLE Customer
(
	Id INT,
	Preferences VARCHAR(MAX),
	Rating INTEGER DEFAULT 0,
	PRIMARY KEY(Id),
	CHECK(Rating >=0 AND Rating <= 10)
)

CREATE TABLE Employee
(
	Id INT,
	SSN INT NOT NULL,
	StartDate DATETIME NOT NULL,
	HourlyRate DECIMAL NOT NULL,
	PRIMARY KEY(Id),
	UNIQUE(SSN),
	CHECK (SSN > 0),
	CHECK (HourlyRate > 0)
)

CREATE TABLE CustomerRep(
	Id INT,
	IsSupervisedBy CHAR(64),
	PRIMARY KEY(Id)
)

CREATE TABLE SiteManager(
	Id INT,
	PRIMARY KEY(Id)
)

CREATE TABLE Account(
	AccountNumber INTEGER  IDENTITY(1,1),
	CreditCardNumber nvarchar(MAX) NOT NULL,
	CustomerId INTEGER NOT NULL,
	DateAdded DATETIME,
	PRIMARY KEY(AccountNumber),
	/* 
		a user should not add two accounts with the same
		credit card
	*/
	
	CHECK(AccountNumber > 0)
)

CREATE TABLE Sale
(
	TransactionId INTEGER IDENTITY(1,1),
	DateOfSale DATETIME NOT NULL,
	NumUnits INTEGER NOT NULL,
	AdId INTEGER NOT NULL,
	AccountNumber INTEGER NOT NULL,
	PRIMARY KEY(TransactionId),
	Check(NumUnits > 0),
	CHECK(TransactionId > 0)
)

CREATE TABLE CustomerBelongsToCircle(
	CircleId INTEGER,
	CustomerId INTEGER NOT NULL,
	UserAccepted BIT NOT NULL DEFAULT 0,
	OwnerAccepted BIT NOT NULL DEFAULT 0,
	PRIMARY KEY(CircleId, CustomerId)
)

CREATE TABLE LikesPost
(
	PostId INTEGER,
	CustomerId INTEGER NOT NULL,
	PRIMARY KEY(PostId, CustomerId)
)

CREATE TABLE LikesComment
(
	CommentId INTEGER NOT NULL,
	CustomerId INTEGER NOT NULL,
	PRIMARY KEY(CommentId, CustomerId)
)

/* Circle*/
ALTER TABLE Circle
	ADD FOREIGN KEY (OwnerId) REFERENCES Customer(Id)
	/* When a user is deleted, delete their circles*/
	ON DELETE CASCADE

/* Page */
ALTER TABLE [Page]
	ADD FOREIGN KEY(CircleId) REFERENCES Circle(Id)
	/* If a circle is deleted, delete its page */
	ON DELETE CASCADE

/* Post */
ALTER TABLE Post
	ADD FOREIGN KEY(AuthorId) REFERENCES Customer(Id)
ALTER TABLE Post
	ADD FOREIGN KEY(PageId) REFERENCES [Page](Id)
	/* If a page is deleted, delete its posts */
	ON DELETE CASCADE

/* Comment */
ALTER TABLE Comment
	ADD FOREIGN KEY(AuthorId) REFERENCES Customer(Id)
	ON DELETE NO ACTION

ALTER TABLE Comment
	ADD FOREIGN KEY(PostId) REFERENCES Post(Id)
	/* If a post is deleted, delete its responses */
	ON DELETE CASCADE

/* Message */
ALTER TABLE PrivateMessage
	ADD FOREIGN KEY(SenderId) REFERENCES Customer(Id)
	/* If a user is deleted, delete their sent messages? */
	ON DELETE CASCADE
ALTER TABLE PrivateMessage
	ADD FOREIGN KEY(RecieverId) REFERENCES Customer(Id)
	ON DELETE NO ACTION

/* Advertisement */
ALTER TABLE Advertisement
	ADD FOREIGN KEY(CreatorId) REFERENCES CustomerRep(Id)
	/* If a Customer Rep is deleted delete their ads */
	ON DELETE CASCADE

/* Customer */
ALTER TABLE Customer
	ADD FOREIGN KEY(Id) REFERENCES [User]
	ON DELETE CASCADE

/* Employee */
ALTER TABLE Employee
	ADD FOREIGN KEY(Id) REFERENCES [User]
	ON DELETE CASCADE

/* CustomerRep */
ALTER TABLE CustomerRep
	ADD FOREIGN KEY(Id) REFERENCES Employee
	ON DELETE CASCADE

/* Site Manager */
ALTER TABLE SiteManager
	ADD FOREIGN KEY(Id) REFERENCES Employee
	ON DELETE CASCADE

/* Account */
ALTER TABLE Account
	ADD FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
	ON DELETE CASCADE

/* Sale */
ALTER TABLE Sale
	ADD FOREIGN KEY(AdId) REFERENCES Advertisement(Id)
	ON DELETE NO ACTION
ALTER TABLE Sale
	ADD FOREIGN KEY(AccountNumber) REFERENCES Account
	ON DELETE NO ACTION
	/* DONT DELETE SALES, need to keep track of those */



/* Belongs To */
ALTER TABLE CustomerBelongsToCircle
	ADD FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
ALTER TABLE CustomerBelongsToCircle
	ADD FOREIGN KEY(CircleId) REFERENCES Circle(Id)
	ON DELETE CASCADE


/* Likes Post */
ALTER TABLE LikesPost
	ADD FOREIGN KEY(PostId) REFERENCES Post(Id)
	ON DELETE CASCADE
ALTER TABLE LikesPost
	ADD FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
	ON DELETE NO ACTION

/* Likes Comment */
ALTER TABLE LikesComment
	ADD FOREIGN KEY(CommentId) REFERENCES Comment(Id)
	ON DELETE CASCADE
ALTER TABLE LikesComment
	ADD FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
	ON DELETE NO ACTION
