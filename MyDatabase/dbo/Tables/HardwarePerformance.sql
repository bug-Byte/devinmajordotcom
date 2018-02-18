CREATE TABLE [dbo].[HardwarePerformance] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
	[HardwareTypeID]   INT  NOT NULL,
    [PercentageValue] FLOAT NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT HardwarePerformance_HardwareTypeID_HardwareType_ID
	FOREIGN KEY([HardwareTypeID]) REFERENCES [HardwareType]([ID])
);

