use devinmajordotcom;

BEGIN TRANSACTION

CREATE SCHEMA Portfolio;

CREATE TABLE Portfolio.[Profile]
(
	ID INT IDENTITY(1,1) NOT NULL,
	FirstName NVARCHAR(100) NOT NULL,
	LastName NVARCHAR(100) NOT NULL,
	DateOfBirth DATETIME NOT NULL,
	[Address] NVARCHAR(100) NOT NULL,
	PhoneNumber NVARCHAR(15) NOT NULL,
	EmailAddress NVARCHAR(200) NOT NULL,
	PositionTitle NVARCHAR(100) NULL,
	PersonalDescription VARCHAR(500) NULL,
	WebsiteText NVARCHAR(100) NULL,
	WebsiteURL NVARCHAR(200) NULL
	PRIMARY KEY(ID)
);
GO

CREATE TABLE Portfolio.[SkillTypeMaster]
(
	ID INT IDENTITY(1,1),
	[TypeName] NVARCHAR(500) NOT NULL,
	PRIMARY KEY(ID)
);
GO

CREATE TABLE Portfolio.[SkillMaster]
(
	ID INT IDENTITY(1,1),
	[Description] NVARCHAR(500) NOT NULL,
	[DisplayName] NVARCHAR(100) NULL,
	SkillTypeID INT NOT NULL,
	PRIMARY KEY(ID),
	CONSTRAINT SkillMaster_SkillTypeID_SkillTypeMaster_ID
	FOREIGN KEY(SkillTypeID) REFERENCES Portfolio.SkillTypeMaster(ID)
);
GO

CREATE TABLE Portfolio.[LanguageTypeMaster]
(
	ID INT IDENTITY(1,1),
	[TypeName] NVARCHAR(500) NOT NULL,
	PRIMARY KEY(ID)
)

CREATE TABLE Portfolio.[LanguageMaster]
(
	ID INT IDENTITY(1,1),
	[Description] NVARCHAR(500) NOT NULL,
	[LanguageTypeID] INT NOT NULL,
	PRIMARY KEY(ID),
	CONSTRAINT LanguageMaster_LanguageTypeID_LanguageTypeMaster_ID
	FOREIGN KEY(LanguageTypeID) REFERENCES Portfolio.LanguageTypeMaster(ID)
);
GO

CREATE TABLE Portfolio.PersonalDescription
(
	ID INT IDENTITY(1,1),
	[Adjective1] NVARCHAR(100) NULL,
	[Adjective2] NVARCHAR(100) NULL,
	[Adjective3] NVARCHAR(100) NULL,
	[Blurb] NVARCHAR(MAX) NULL,
	PRIMARY KEY(ID)
);
GO

COMMIT