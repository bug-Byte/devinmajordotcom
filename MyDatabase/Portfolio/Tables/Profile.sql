CREATE TABLE [Portfolio].[Profile] (
    [ID]                  INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]           NVARCHAR (100) NOT NULL,
    [LastName]            NVARCHAR (100) NOT NULL,
    [DateOfBirth]         DATETIME       NOT NULL,
    [Address]             NVARCHAR (100) NOT NULL,
    [PhoneNumber]         NVARCHAR (25)  NOT NULL,
    [EmailAddress]        NVARCHAR (200) NOT NULL,
    [PositionTitle]       NVARCHAR (100) NULL,
    [PersonalDescription] VARCHAR (500)  NULL,
    [WebsiteText]         NVARCHAR (100) NULL,
    [WebsiteURL]          NVARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

