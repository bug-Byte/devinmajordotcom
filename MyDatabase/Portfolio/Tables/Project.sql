CREATE TABLE [Portfolio].[Project] (
    [ID]          INT             IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100)  NOT NULL,
    [Description] NVARCHAR (500)  NULL,
    [Image]       VARBINARY (MAX) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

