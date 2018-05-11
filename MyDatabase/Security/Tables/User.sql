CREATE TABLE [Security].[User] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [ClientName]   NVARCHAR (500) NOT NULL,
    [IsActive]     BIT            DEFAULT ((1)) NOT NULL,
    [EmailAddress] NVARCHAR (250) NULL,
    [IsAdmin]      BIT            DEFAULT ((0)) NOT NULL,
	[IsEmailConfirmed]      BIT            DEFAULT ((0)) NOT NULL,
    [UserName] NVARCHAR(MAX) NULL, 
    [Password] NVARCHAR(MAX) NULL, 
    [GUID] UNIQUEIDENTIFIER NOT NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

