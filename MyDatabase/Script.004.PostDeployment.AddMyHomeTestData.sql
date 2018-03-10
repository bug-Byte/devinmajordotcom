/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

BEGIN TRANSACTION

INSERT INTO [Security].[User]
(
	[ClientName]
	,[IsActive]
	,[EmailAddress]
	,[IsAdmin]
	,[UserName]
	,[Password]
	,[GUID]
)
VALUES
(
	'::1',
	0,
	NULL,
	0,
	'Guest',
	NULL,
	NEWID()
);
GO

DECLARE @GuestUserID INT = (SELECT ID FROM [Security].[User] WHERE UserName='Guest' AND ClientName='::1' AND IsActive=0);


INSERT INTO [MyHome].[UserConfig]
(
	[UserID]
	,[ShowDateAndTime]
	,[ShowWeather]
	,[ShowBanner]
	,[ShowBookmarks]
	,[ShowBlog]
	,[BookmarksTitle]
	,[Greeting]
	,[BlogTitle]
	,[BackgroundImage]
	,[ShowVisitorsAdminHome]
	,[IsEditable]
)
VALUES
(
	@GuestUserID,
	1,
	1,
	1,
	1,
	1,
	'My Favorites & Bookmarks',
	'Welcome Home, Devin!',
	'My Blog & Notes',
	(SELECT * FROM OPENROWSET(BULK '$(ProjectLocation)\devinmajordotcom\Content\HomeImages\home-bg4.jpg', SINGLE_BLOB) AS [BackgroundImage]),
	0,
	0
);

DECLARE @Links TABLE(ID INT IDENTITY(1,1) NOT NULL, [Name] VARCHAR(MAX) NOT NULL, [ImgPathName] VARCHAR(MAX) NOT NULL, [Img] VARBINARY(MAX) NULL, [URL] VARCHAR(MAX) NOT NULL);
DECLARE @Counter1 INT = 1;

INSERT INTO @Links([Name], [ImgPathName], [URL]) 
VALUES 
('Facebook', 'facebook.png', 'https://www.facebook.com'), 
('Reddit', 'reddit.jpg', 'https://www.reddit.com/'), 
('YouTube', 'youtube.png', 'https://www.youtube.com/'), 
('Plex Media Dashboard', 'plex.jpg', 'http://www.devinmajor.com/MediaDashboard'), 
('GitHub', 'github.png', 'https://github.com/'), 
('Slack', 'slack.jpg', 'https://slack.com/'), 
('Amazon', 'amazon1.jpg', 'https://www.amazon.ca/'),
('Newegg', 'newegg.jpg', 'https://www.newegg.ca/'), 
('Outlook', 'outlook.jpg', 'https://outlook.live.com/owa/');

WHILE(@Counter1 <= (SELECT MAX(ID) FROM @Links))
BEGIN	
	DECLARE @ImageName VARCHAR(MAX) = (SELECT [ImgPathName] FROM @Links WHERE ID = @Counter1);
	DECLARE @ImageQuery NVARCHAR(MAX) = 'SELECT @img = (SELECT * FROM OPENROWSET(BULK ''$(ProjectLocation)\devinmajordotcom\Content\HomeImages\' + @ImageName + ''', SINGLE_BLOB) AS [Image])';
	DECLARE @Image VARBINARY(MAX);	
	EXEC sp_executesql @ImageQuery, N'@img VARBINARY(MAX) OUTPUT', @img=@Image OUTPUT;
	UPDATE @Links SET [Img] = @Image WHERE ID = @Counter1;
	SET @Counter1 = @Counter1 + 1;
END

INSERT INTO [MyHome].[SiteLink]
(
	[UserID]
    ,[DisplayName]
    ,[Description]
    ,[Directive]
    ,[URL]
    ,[Action]
    ,[Controller]
    ,[DisplayIcon]
    ,[IsDefault]
    ,[IsEnabled]
	,[Image]
    ,[Order]
)
(
	SELECT 
		@GuestUserID,
		t.Name,
		NULL,
		NULL,
		t.[URL],
		NULL,
		NULL,
		NULL,
		0,
		1,
		t.[Img],
		1
	FROM @Links t
);
GO

COMMIT