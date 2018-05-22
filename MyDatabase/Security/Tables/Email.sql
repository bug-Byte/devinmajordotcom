CREATE TABLE [Security].[Email]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	EmailTypeID INT NOT NULL,
	[SenderUserGUID] UNIQUEIDENTIFIER NOT NULL,
	SenderEmailAddress VARCHAR(MAX) NULL,
	RecipientEmail VARCHAR(MAX) NULL,
	RecipientName VARCHAR(MAX) NULL,
	SenderName VARCHAR(MAX) NULL,
	[Subject] VARCHAR(MAX) NULL,
	Content VARCHAR(MAX) NULL,
	PRIMARY KEY (Id),
	CONSTRAINT [Security_Email_EmailTypeID_Security_EmailType_ID] FOREIGN KEY (EmailTypeID) REFERENCES [Security].[EmailType] ([ID])
)
