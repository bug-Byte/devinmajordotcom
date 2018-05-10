CREATE TABLE [MediaDashboard].[UserConfig]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	UserID INT NOT NULL,
	SidebarFullTitle VARCHAR(MAX) NULL,
	SidebarCollapsedTitle VARCHAR(MAX) NULL,
	BackgroundImage VARCHAR(MAX) NULL,
	SidebarColor VARCHAR(MAX) NULL,
	SidebarAccentColor VARCHAR(MAX) NULL,
	WebsiteTitle VARCHAR(MAX) NULL,
	PRIMARY KEY(Id),
	CONSTRAINT MediaDashboard_UserConfig_UserID_Security_User_ID
	FOREIGN KEY(UserID) REFERENCES [Security].[User](ID)
)
