CREATE TABLE [MyHome].[UserConfig]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	UserID INT NOT NULL,
	ShowDateAndTime BIT NOT NULL,	
	ShowWeather BIT NOT NULL,
	ShowBanner BIT NOT NULL,
	ShowBookmarks BIT NOT NULL,
	ShowBlog BIT NOT NULL,
	WebsiteName VARCHAR(MAX) NULL,
	BookmarksTitle VARCHAR(MAX) NULL,
	Greeting VARCHAR(MAX) NOT NULL,
	BlogTitle VARCHAR(MAX) NULL,
	BackgroundImage VARBINARY (MAX) NULL,
	IsEditable BIT NOT NULL DEFAULT 1,
	ShowVisitorsAdminHome BIT NOT NULL,
	DefaultFavoriteImage VARBINARY(MAX) NULL,
	DefaultBlogPostImage VARBINARY(MAX) NULL,
	AddNewFavoriteImage VARBINARY(MAX) NULL,
	PRIMARY KEY(Id),
	CONSTRAINT MyHome_UserHomeConfig_UserId_Security_User_ID
	FOREIGN KEY(UserID) REFERENCES [Security].[User](ID)
)
