-- Database

CREATE DATABASE OnlineShop;
GO

-- Use

USE OnlineShop;

-- Baza Danych

CREATE TABLE Users(
ID VARCHAR(48) NOT NULL PRIMARY KEY,
AccountRole INT NOT NULL,
Name VARCHAR(64) NOT NULL,
Email VARCHAR(64) UNIQUE NOT NULL,
Password VARCHAR(64) NOT NULL,
CreatedAt DATETIME NOT NULL
); 

CREATE TABLE Products(
ID VARCHAR(48) NOT NULL PRIMARY KEY,
Name VARCHAR(64) UNIQUE NOT NULL,
Description VARCHAR(256) NOT NULL,
CreatedAt DATETIME NOT NULL,
UpdatedAt DATETIME NOT NULL,
); 

CREATE TABLE SingleProductCopy(
ID VARCHAR(48) NOT NULL PRIMARY KEY,
ProductID VARCHAR(48) NOT NULL,
Price DECIMAL(18,2) NOT NULL,
UserId VARCHAR(48) NULL,
UserName VARCHAR(64) NULL,
PurchasedAt DATETIME NULL
);

-- Log

CREATE TABLE LOGUsers(
LogDate DATETIME NOT NULL,
CommandType VARCHAR(7) NOT NULL,
ID VARCHAR(48) NOT NULL,
AccountRole INT NULL,
Name VARCHAR(64) NULL,
Email VARCHAR(64) NULL,
Password VARCHAR(64) NULL,
CreatedAt DATETIME NULL
); 

CREATE TABLE LOGProducts(
LogDate DATETIME NOT NULL,
CommandType VARCHAR(7) NOT NULL,
ID VARCHAR(48) NOT NULL,
Name VARCHAR(64) NULL,
Description VARCHAR(256) NULL,
CreatedAt DATETIME NULL,
UpdatedAt DATETIME NULL,
); 

CREATE TABLE LOGSingleProductCopy(
LogDate DATETIME NOT NULL,
CommandType VARCHAR(7) NOT NULL,
ID VARCHAR(48) NOT NULL,
ProductID VARCHAR(48) NOT NULL,
Price DECIMAL(18,2) NULL,
UserId VARCHAR(48) NULL,
UserName VARCHAR(64) NULL,
PurchasedAt DATETIME NULL
);

-- Klucz obcy

ALTER TABLE SingleProductCopy
ADD CONSTRAINT FK_SingleProductCopy_Products FOREIGN KEY (ProductID)
REFERENCES Products (ID);

ALTER TABLE SingleProductCopy
ADD CONSTRAINT FK_SingleProductCopy_Users FOREIGN KEY (UserId)
REFERENCES Users (ID);

-- Index

CREATE NONCLUSTERED INDEX Email_Index ON Users (Email);
CREATE NONCLUSTERED INDEX Name_Index ON Products (Name);

-- Trigger
GO

CREATE TRIGGER UsersInsert
ON Users
AFTER INSERT
AS
	INSERT INTO LOGUsers VALUES 
	(GETDATE(), 
	'INSERT',
	(SELECT ID FROM inserted), 
	(SELECT AccountRole FROM inserted),
	(SELECT Name FROM inserted),
	(SELECT Email FROM inserted),
	(SELECT Password FROM inserted),
	(SELECT CreatedAt FROM inserted));
GO

CREATE TRIGGER UsersUpdate
ON Users
AFTER UPDATE
AS
	INSERT INTO LOGUsers VALUES 
	(GETDATE(), 
	'UPDATE',
	(SELECT ID FROM inserted), 
	(SELECT AccountRole FROM inserted),
	(SELECT Name FROM inserted),
	(SELECT Email FROM inserted),
	(SELECT Password FROM inserted),
	(SELECT CreatedAt FROM inserted));
GO

CREATE TRIGGER UsersDelete
ON Users
AFTER DELETE
AS
	INSERT INTO LOGUsers (LogDate, CommandType, ID) VALUES (GETDATE(), 'DELETE', (SELECT ID FROM deleted));
GO


CREATE TRIGGER ProductsInsert
ON Products
AFTER INSERT
AS
	INSERT INTO LOGProducts VALUES 
	(GETDATE(), 
	'INSERT',
	(SELECT ID FROM inserted), 
	(SELECT Name FROM inserted),
	(SELECT Description FROM inserted),
	(SELECT CreatedAt FROM inserted),
	(SELECT UpdatedAt FROM inserted));
GO

CREATE TRIGGER ProductsUpdate
ON Products
AFTER UPDATE
AS
	INSERT INTO LOGProducts VALUES 
	(GETDATE(), 
	'UPDATE',
	(SELECT ID FROM inserted), 
	(SELECT Name FROM inserted),
	(SELECT Description FROM inserted),
	(SELECT CreatedAt FROM inserted),
	(SELECT UpdatedAt FROM inserted));
GO

CREATE TRIGGER ProductsDelete
ON Products
AFTER DELETE
AS
	INSERT INTO LOGProducts (LogDate, CommandType, ID) VALUES (GETDATE(), 'DELETE', (SELECT ID FROM deleted));
GO


CREATE TRIGGER SingleProductCopyInsert
ON SingleProductCopy
AFTER INSERT
AS
	INSERT INTO LOGSingleProductCopy VALUES 
	(GETDATE(), 
	'INSERT',
	(SELECT ID FROM inserted), 
	(SELECT ProductID FROM inserted),
	(SELECT Price FROM inserted),
	(SELECT UserId FROM inserted),
	(SELECT UserName FROM inserted),
	(SELECT PurchasedAt FROM inserted));
GO

CREATE TRIGGER SingleProductCopyUpdate
ON SingleProductCopy
AFTER UPDATE
AS
	INSERT INTO LOGSingleProductCopy VALUES 
	(GETDATE(), 
	'UPDATE',
	(SELECT ID FROM inserted), 
	(SELECT ProductID FROM inserted),
	(SELECT Price FROM inserted),
	(SELECT UserId FROM inserted),
	(SELECT UserName FROM inserted),
	(SELECT PurchasedAt FROM inserted));
GO

CREATE TRIGGER SingleProductCopyDelete
ON SingleProductCopy
AFTER DELETE
AS
	INSERT INTO LOGSingleProductCopy (LogDate, CommandType, ID, ProductID) VALUES 
	(GETDATE(), 'DELETE', (SELECT ID FROM deleted), (SELECT ProductID FROM deleted));
GO