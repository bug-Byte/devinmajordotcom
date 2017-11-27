﻿CREATE TABLE [dbo].[User] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [ClientName]   NVARCHAR (500) NOT NULL,
    [IsActive]     BIT            DEFAULT ((1)) NOT NULL,
    [EmailAddress] NVARCHAR (250) NULL,
    [IsAdmin]      BIT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);
