﻿CREATE TABLE [LandingPage].[Config]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	AppsTitle VARCHAR(MAX) NULL,
	BackgroundImage VARBINARY (MAX) NULL,
	IsParticleCanvasOn BIT NOT NULL DEFAULT 0
	PRIMARY KEY(Id)
)
