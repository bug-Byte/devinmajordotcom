CREATE TABLE [Portfolio].[Config]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	WebsiteTitle VARCHAR(MAX) NULL,
	BackgroundImage VARBINARY(MAX) NULL,
	PRIMARY KEY(Id)
)
