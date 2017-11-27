CREATE TABLE [Portfolio].[PersonalDescription] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [Adjective1] NVARCHAR (100) NULL,
    [Adjective2] NVARCHAR (100) NULL,
    [Adjective3] NVARCHAR (100) NULL,
    [Blurb]      NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

