CREATE TABLE [Portfolio].[LanguageSkill] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [Description]           NVARCHAR (500) NULL,
    [DisplayName]           NVARCHAR (100) NOT NULL,
    [ProficiencyPercentage] INT            NULL,
    [DisplayIcon]           NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

