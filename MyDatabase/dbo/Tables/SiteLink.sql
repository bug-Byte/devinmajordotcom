CREATE TABLE [dbo].[SiteLink] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [DisplayName]   NVARCHAR (50)  NOT NULL,
    [Description]   NVARCHAR (500) NULL,
    [Directive]     NVARCHAR (100) NULL,
    [URL]           NVARCHAR (500) NULL,
    [Action]        NVARCHAR (500) NULL,
    [Controller]    NVARCHAR (500) NULL,
    [DisplayIcon]   NVARCHAR (500) NULL,
    [IsDefault]     BIT            DEFAULT ((0)) NOT NULL,
    [IsEnabled]     BIT            DEFAULT ((1)) NOT NULL,
    [ApplicationID] INT            NOT NULL,
    [Order]         INT            NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [SiteLinks_ApplicationID_Applications_ID] FOREIGN KEY ([ApplicationID]) REFERENCES [dbo].[ApplicationMaster] ([ID])
);

