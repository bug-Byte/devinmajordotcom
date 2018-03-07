CREATE TABLE [MyHome].[SiteLink] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
	[UserID]		INT NOT NULL,
    [DisplayName]   NVARCHAR (50)  NOT NULL,
    [Description]   NVARCHAR (500) NULL,
    [Directive]     NVARCHAR (100) NULL,
    [URL]           NVARCHAR (500) NULL,
    [Action]        NVARCHAR (500) NULL,
    [Controller]    NVARCHAR (500) NULL,
    [DisplayIcon]   NVARCHAR (500) NULL,
    [IsDefault]     BIT            DEFAULT ((0)) NOT NULL,
    [IsEnabled]     BIT            DEFAULT ((1)) NOT NULL,
	[Image] VARBINARY(MAX) NULL,
    [Order]         INT            NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT MyHome_SiteLink_UserID_Security_User_Id
	FOREIGN KEY(UserID) REFERENCES [Security].[User](ID)
);

