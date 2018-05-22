CREATE TABLE [Security].[Email]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[SenderUserID] INT NOT NULL,
	SenderEmailAddress VARCHAR(MAX) NULL,
	RecipientEmail VARCHAR(MAX) NULL,
	RecipientName VARCHAR(MAX) NULL,
	SenderName VARCHAR(MAX) NULL,
	[Subject] VARCHAR(MAX) NULL,
	Content VARCHAR(MAX) NULL,
	PRIMARY KEY (Id),
	CONSTRAINT [Security_Email_SenderUserID_Security_User_ID] FOREIGN KEY ([SenderUserID]) REFERENCES [Security].[User] ([ID])
)
