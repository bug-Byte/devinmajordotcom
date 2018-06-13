CREATE TABLE [Security].[HardwarePerformance] (
    [ID]   INT           IDENTITY (1, 1) NOT NULL,
	[HardwareTypeID]   INT  NOT NULL,
    [PercentageValue] FLOAT NOT NULL,
	HardwareName VARCHAR(MAX) NULL,
	HardwareNumber INT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT HardwarePerformance_HardwareTypeID_HardwareType_ID
	FOREIGN KEY([HardwareTypeID]) REFERENCES [Security].[HardwareType]([ID])
);