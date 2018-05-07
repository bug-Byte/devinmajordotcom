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
	,[WebsiteName]
	,[BookmarksTitle]
	,[Greeting]
	,[BlogTitle]
	,[BackgroundImage]
	,[ShowVisitorsAdminHome]
	,[IsEditable]
	,[DefaultFavoriteImage]
	,[DefaultBlogPostImage]
	,[AddNewFavoriteImage]
)
VALUES
(
	@GuestUserID,
	1,
	1,
	1,
	1,
	1,
	'MyHome',
	'My Favorites & Bookmarks',
	'Welcome Home, Devin!',
	'My Blog & Notes',
	'Content/HomeImages/home-bg4.jpg',
	0,
	0,
	'Content/HomeImages/DefaultFavoriteImage.png',
	'Content/HomeImages/blog-image1.jpg',
	'Content/HomeImages/AddNewFavorite.png'
);
GO

DECLARE @GuestUserID INT = (SELECT ID FROM [Security].[User] WHERE UserName='Guest' AND ClientName='::1' AND IsActive=0);

DECLARE @Posts TABLE(ID INT IDENTITY(1,1) NOT NULL, [Title] VARCHAR(MAX) NOT NULL, [Body] VARCHAR(MAX) NOT NULL, [ImgPathName] VARCHAR(MAX) NOT NULL, [Img] VARBINARY(MAX) NULL);
DECLARE @Links TABLE(ID INT IDENTITY(1,1) NOT NULL, [Name] VARCHAR(MAX) NOT NULL, [ImgPathName] VARCHAR(MAX) NOT NULL, [Img] VARBINARY(MAX) NULL, [URL] VARCHAR(MAX) NOT NULL);


INSERT INTO @Posts([Title], [Body], [ImgPathName]) 
VALUES 
('Need to start a game, maybe in Unity?', 'Also thinking about DX12 since I''m in possession of a GTX 1080Ti!', 'Content/HomeImages/home-bg4.jpg'), 
('RAM Prices SUCK right now!', 'I cant believe this! All I want is 16 gigs of DDR4, enough to run my poor server :(', 'Content/HomeImages/blog-image2.jpg'),
('I need a new hobby lol', 'This is ridiculous. I cant spend all my time coding, lol. Oh well.', 'Content/HomeImages/blog-image3.jpg'), 
('Tim Hortons Roll-up the Rim is Such a Scam', 'Its true and we all know it. Why even bother? Some people I tell ya.', 'Content/HomeImages/blog-image4.jpg');

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
		p.[ImgPathName],
		p.[Title],
		p.[Body]
	FROM @Posts p
);

INSERT INTO @Links([Name], [ImgPathName], [URL]) 
VALUES 
('Facebook', 'Content/HomeImages/facebook.png', 'https://www.facebook.com'), 
('Reddit', 'Content/HomeImages/reddit.jpg', 'https://www.reddit.com/'), 
('YouTube', 'Content/HomeImages/youtube.png', 'https://www.youtube.com/'), 
('Plex Media Dashboard', 'Content/HomeImages/plex.jpg', 'http://www.devinmajor.com/MediaDashboard'), 
('GitHub', 'Content/HomeImages/github.png', 'https://github.com/'), 
('Slack', 'Content/HomeImages/slack.jpg', 'https://slack.com/'), 
('Amazon', 'Content/HomeImages/amazon1.jpg', 'https://www.amazon.ca/'),
('Newegg', 'Content/HomeImages/newegg.jpg', 'https://www.newegg.ca/'), 
('Outlook', 'Content/HomeImages/outlook.jpg', 'https://outlook.live.com/owa/');

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
		t.[ImgPathName],
		1
	FROM @Links t
);
GO

COMMIT