CREATE TABLE [Portfolio].[Skill] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [Description]           NVARCHAR (500) NOT NULL,
    [DisplayName]           NVARCHAR (100) NULL,
    [ProficiencyPercentage] INT            NULL,
    [DisplayIcon]           NVARCHAR (100) NULL,
    [SkillTypeID]           INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [SkillMaster_SkillTypeID_SkillTypeMaster_ID] FOREIGN KEY ([SkillTypeID]) REFERENCES [Portfolio].[SkillTypeMaster] ([ID])
);

