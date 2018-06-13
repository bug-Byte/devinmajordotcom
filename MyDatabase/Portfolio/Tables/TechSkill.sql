CREATE TABLE [Portfolio].[TechSkill] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [Description]           NVARCHAR (500) NOT NULL,
    [DisplayName]           NVARCHAR (100) NULL,
    [ProficiencyPercentage] INT            NULL,
    [DisplayIcon]           NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

