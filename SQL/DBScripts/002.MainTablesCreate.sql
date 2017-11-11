use [#dbname]


EXEC sp_configure 'show advanced options', 1 ;   
RECONFIGURE ;   
GO  
EXEC sp_configure 'max text repl size', -1 ;   
GO  
RECONFIGURE;   
GO  

BEGIN TRANSACTION

CREATE TABLE [User]
(
	ID INT IDENTITY(1,1),
	ClientName NVARCHAR(500) NOT NULL,
	IsActive BIT NOT NULL DEFAULT 1,
	IsAdmin BIT NOT NULL DEFAULT 0,
	PRIMARY KEY(ID)
)

CREATE TABLE ApplicationMaster (
	ID INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	PRIMARY KEY (ID)
);
GO

CREATE TABLE SiteLink (

	ID INT IDENTITY(1,1) NOT NULL,
	DisplayName NVARCHAR(50) NOT NULL,
	[Description] NVARCHAR(500) NULL,
	[Directive] NVARCHAR(100) NULL,
	[URL] NVARCHAR(500) NULL,
	[Action] NVARCHAR(500) NULL,
	[Controller] NVARCHAR(500) NULL,
	DisplayIcon NVARCHAR(500) NULL,
	IsDefault BIT NOT NULL DEFAULT 0,
	IsEnabled BIT NOT NULL DEFAULT 1,
	ApplicationID INT NOT NULL,
	[Order] INT NULL,
	PRIMARY KEY (ID),
	CONSTRAINT SiteLinks_ApplicationID_Applications_ID
	FOREIGN KEY(ApplicationID) REFERENCES ApplicationMaster(ID) 
);
GO

COMMIT