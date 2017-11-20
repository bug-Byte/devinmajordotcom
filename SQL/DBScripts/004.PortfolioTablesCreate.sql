use [#dbname];

BEGIN TRANSACTION

EXEC sp_executesql N'CREATE SCHEMA Portfolio;';
GO

CREATE TABLE Portfolio.[Profile]
(
	ID INT IDENTITY(1,1) NOT NULL,
	FirstName NVARCHAR(100) NOT NULL,
	LastName NVARCHAR(100) NOT NULL,
	DateOfBirth DATETIME NOT NULL,
	[Address] NVARCHAR(100) NOT NULL,
	PhoneNumber NVARCHAR(25) NOT NULL,
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

CREATE TABLE Portfolio.[Skill]
(
	ID INT IDENTITY(1,1),
	[Description] NVARCHAR(500) NOT NULL,
	[DisplayName] NVARCHAR(100) NULL,
	[ProficiencyPercentage] INT NULL,
	[DisplayIcon] NVARCHAR(100) NULL,
	SkillTypeID INT NOT NULL,
	PRIMARY KEY(ID),
	CONSTRAINT SkillMaster_SkillTypeID_SkillTypeMaster_ID
	FOREIGN KEY(SkillTypeID) REFERENCES Portfolio.SkillTypeMaster(ID)
);
GO
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

CREATE TABLE Portfolio.ProjectType
(
	ID INT IDENTITY(1,1),
	[Type] NVARCHAR(100) NOT NULL,
	PRIMARY KEY(ID)
);
GO

CREATE TABLE Portfolio.Project
(
	ID INT IDENTITY(1,1),
	[Name] NVARCHAR(100) NOT NULL,
	[Description] NVARCHAR(500) NULL,
	[Image] VARBINARY(MAX) NULL,
	PRIMARY KEY(ID)
);
GO

CREATE TABLE Portfolio.ProjectTypeMapping
(
	ID INT IDENTITY(1,1),
	ProjectID INT NOT NULL,
	ProjectTypeID INT NOT NULL,
	PRIMARY KEY(ID),
	CONSTRAINT ProjectTypeMapping_ProjectID_Project_ID
	FOREIGN KEY(ProjectID) REFERENCES Portfolio.Project(ID),
	CONSTRAINT ProjectTypeMapping_ProjectTypeID_ProjectType_ID
	FOREIGN KEY(ProjectTypeID) REFERENCES Portfolio.ProjectType(ID)
);
GO

COMMIT