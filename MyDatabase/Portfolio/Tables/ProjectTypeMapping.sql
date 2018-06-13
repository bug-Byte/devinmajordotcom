CREATE TABLE [Portfolio].[ProjectTypeMapping] (
    [ID]            INT IDENTITY (1, 1) NOT NULL,
    [ProjectID]     INT NOT NULL,
    [ProjectTypeID] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [ProjectTypeMapping_ProjectID_Project_ID] FOREIGN KEY ([ProjectID]) REFERENCES [Portfolio].[Project] ([ID]),
    CONSTRAINT [ProjectTypeMapping_ProjectTypeID_ProjectType_ID] FOREIGN KEY ([ProjectTypeID]) REFERENCES [Portfolio].[ProjectType] ([ID])
);

