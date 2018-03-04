CREATE TABLE [LandingPage].[Config]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	AppsTitle VARCHAR(MAX) NULL,
	AppsIntro VARCHAR(MAX) NULL,
	AppsDescription VARCHAR(MAX) NULL,
	ContactTitle VARCHAR(MAX) NULL,
	ContactInstructions VARCHAR(MAX) NULL,
	ServerStatusTitle VARCHAR(MAX) NULL,
	ServerStatusDescription VARCHAR(MAX) NULL
	PRIMARY KEY(Id)
)
