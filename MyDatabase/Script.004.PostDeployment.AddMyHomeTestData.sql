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
GO

DECLARE @GuestUserID INT = (SELECT ID FROM [Security].[User] WHERE UserName='Guest' AND ClientName='::1' AND IsActive=0);

DECLARE @Counter1 INT = 1;
DECLARE @Counter2 INT = 1;

DECLARE @Posts TABLE(ID INT IDENTITY(1,1) NOT NULL, [Title] VARCHAR(MAX) NOT NULL, [Body] VARCHAR(MAX) NOT NULL, [ImgPathName] VARCHAR(MAX) NOT NULL, [Img] VARBINARY(MAX) NULL);
DECLARE @Links TABLE(ID INT IDENTITY(1,1) NOT NULL, [Name] VARCHAR(MAX) NOT NULL, [ImgPathName] VARCHAR(MAX) NOT NULL, [Img] VARBINARY(MAX) NULL, [URL] VARCHAR(MAX) NOT NULL);


INSERT INTO @Posts([Title], [Body], [ImgPathName]) 
VALUES 
('Need to start a game, maybe in Unity?', 'Also thinking about DX12 since I''m in possession of a GTX 1080Ti!', 'home-bg4.jpg'), 
('RAM Prices SUCK right now!', 'I cant believe this! All I want is 16 gigs of DDR4, enough to run my poor server :(', 'blog-image2.jpg'),
('I need a new hobby lol', 'This is ridiculous. I cant spend all my time coding, lol. Oh well.', 'blog-image3.jpg'), 
('Tim Hortons Roll-up the Rim is Such a Scam', 'Its true and we all know it. Why even bother? Some people I tell ya.', 'blog-image4.jpg');

WHILE(@Counter2 <= (SELECT MAX(ID) FROM @Posts))
BEGIN	
	DECLARE @ImageName1 VARCHAR(MAX) = (SELECT [ImgPathName] FROM @Posts WHERE ID = @Counter2);
	DECLARE @ImageQuery1 NVARCHAR(MAX) = 'SELECT @img = (SELECT * FROM OPENROWSET(BULK ''$(ProjectLocation)\devinmajordotcom\Content\HomeImages\' + @ImageName1 + ''', SINGLE_BLOB) AS [Image])';
	DECLARE @Image1 VARBINARY(MAX);	
	EXEC sp_executesql @ImageQuery1, N'@img VARBINARY(MAX) OUTPUT', @img=@Image1 OUTPUT;
	UPDATE @Posts SET [Img] = @Image1 WHERE ID = @Counter2;
	SET @Counter2 = @Counter2 + 1;
END

INSERT INTO [MyHome].[BlogPost]
(
	[UserID]
    ,[Image]
    ,[Title]
    ,[Body]
)
(
	SELECT 
		@GuestUserID,
		p.Img,
		p.[Title],
		p.[Body]
	FROM @Posts p
);

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
	DECLARE @ImageName2 VARCHAR(MAX) = (SELECT [ImgPathName] FROM @Links WHERE ID = @Counter1);
	DECLARE @ImageQuery2 NVARCHAR(MAX) = 'SELECT @img = (SELECT * FROM OPENROWSET(BULK ''$(ProjectLocation)\devinmajordotcom\Content\HomeImages\' + @ImageName2 + ''', SINGLE_BLOB) AS [Image])';
	DECLARE @Image2 VARBINARY(MAX);	
	EXEC sp_executesql @ImageQuery2, N'@img VARBINARY(MAX) OUTPUT', @img=@Image2 OUTPUT;
	UPDATE @Links SET [Img] = @Image2 WHERE ID = @Counter1;
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

DECLARE @GuestUserID INT = (SELECT ID FROM [Security].[User] WHERE UserName='Guest' AND ClientName='::1' AND IsActive=0);

INSERT INTO [MediaDashboard].[UserConfig]
(
	UserID,
	SidebarFullTitle,
	SidebarCollapsedTitle
	--,BackgroundImage
	,SidebarColor
	,SidebarAccentColor
)
VALUES
(
	@GuestUserID,
	'My Media Dashboard',
	'M D',
	'black',
	'#28a08c'
);
GO

COMMIT