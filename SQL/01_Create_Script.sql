use [master]
GO

DROP DATABASE IF EXISTS GamePoc
GO

CREATE DATABASE GamePoc
GO

use [GamePoc]
GO

CREATE TABLE Item (
	Id int PRIMARY KEY IDENTITY(1, 1),
	[Name] nvarchar(20) UNIQUE,
	SellPrice int
)

CREATE TABLE Generator (
	Id int PRIMARY KEY IDENTITY(1, 1),
	[Name] nvarchar(25) UNIQUE,
	BuyPrice int,
	ItemId int,
	Amount int,
	SPG int -- Seconds per Generation
	
	CONSTRAINT [FK_Generator_Item_Id] FOREIGN KEY (ItemId) REFERENCES Item(Id)
)

CREATE TABLE Backpack (
	Id int PRIMARY KEY IDENTITY(1, 1),
	[Name] nvarchar(20) UNIQUE,
	BuyPrice int,
	ItemLimit int
)

CREATE TABLE UserProfile (
	Id int PRIMARY KEY IDENTITY(1, 1),
	DiscordUid nvarchar(max),
	DateCreated datetime DEFAULT(GETDATE()),
	Balance int DEFAULT(0),
	GeneratorId int DEFAULT(1),
	BackpackId int DEFAULT(1),
	LastSell datetime DEFAULT(GETDATE()),
	DateDeleted datetime

	CONSTRAINT [FK_UserProfile_Generator_Id] FOREIGN KEY (GeneratorId) REFERENCES Generator(Id),
	CONSTRAINT [FK_UserProfile_Backpack_Id] FOREIGN KEY (BackpackId) REFERENCES Backpack(Id)
)

-- Table for general user data
-- Table for generators (id, itemGenerated, rate)
-- Table for backpacks?