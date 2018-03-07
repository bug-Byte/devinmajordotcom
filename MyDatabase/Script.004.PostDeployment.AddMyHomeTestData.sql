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

DECLARE @Path VARCHAR(MAX) = [$(ProjectLocation)];
DECLARE @GuestUserID INT = (SELECT ID FROM [Security].[User] WHERE UserName='Guest' AND ClientName='::1' AND IsActive=0);

DECLARE @FacebookImageQuery NVARCHAR(MAX) = CONCAT('SELECT @FbImage = (SELECT * FROM OPENROWSET(BULK ''', @Path, '\devinmajordotcom\Content\HomeImages\facebook.png'', SINGLE_BLOB) AS [Image])');
DECLARE @FacebookImage VARBINARY(MAX);
exec sp_executesql @FacebookImageQuery, N'@FbImage VARBINARY(MAX) OUTPUT', @FbImage=@FacebookImage OUTPUT;

DECLARE @RedditImageQuery NVARCHAR(MAX) = CONCAT('SELECT @redImage = (SELECT * FROM OPENROWSET(BULK ''', @Path, '\devinmajordotcom\Content\HomeImages\reddit.png'', SINGLE_BLOB) AS [Image])');
DECLARE @RedditImage VARBINARY(MAX);
exec sp_executesql @RedditImageQuery, N'@redImage VARBINARY(MAX) OUTPUT', @redImage=@RedditImage OUTPUT;

DECLARE @FacebookImageQuery NVARCHAR(MAX) = CONCAT('SELECT @slackImage = (SELECT * FROM OPENROWSET(BULK ''', @Path, '\devinmajordotcom\Content\HomeImages\slack.png'', SINGLE_BLOB) AS [Image])');
DECLARE @FacebookImage VARBINARY(MAX);
exec sp_executesql @FacebookImageQuery, N'@slackImage VARBINARY(MAX) OUTPUT', @slackImage=@FacebookImage OUTPUT;

DECLARE @FacebookImageQuery NVARCHAR(MAX) = CONCAT('SELECT @FbImage = (SELECT * FROM OPENROWSET(BULK ''', @Path, '\devinmajordotcom\Content\HomeImages\reddit.png'', SINGLE_BLOB) AS [Image])');
DECLARE @FacebookImage VARBINARY(MAX);
exec sp_executesql @FacebookImageQuery, N'@FbImage VARBINARY(MAX) OUTPUT', @FbImage=@FacebookImage OUTPUT;

DECLARE @FacebookImageQuery NVARCHAR(MAX) = CONCAT('SELECT @FbImage = (SELECT * FROM OPENROWSET(BULK ''', @Path, '\devinmajordotcom\Content\HomeImages\reddit.png'', SINGLE_BLOB) AS [Image])');
DECLARE @FacebookImage VARBINARY(MAX);
exec sp_executesql @FacebookImageQuery, N'@FbImage VARBINARY(MAX) OUTPUT', @FbImage=@FacebookImage OUTPUT;

DECLARE @FacebookImageQuery NVARCHAR(MAX) = CONCAT('SELECT @FbImage = (SELECT * FROM OPENROWSET(BULK ''', @Path, '\devinmajordotcom\Content\HomeImages\reddit.png'', SINGLE_BLOB) AS [Image])');
DECLARE @FacebookImage VARBINARY(MAX);
exec sp_executesql @FacebookImageQuery, N'@FbImage VARBINARY(MAX) OUTPUT', @FbImage=@FacebookImage OUTPUT;

DECLARE @FacebookImageQuery NVARCHAR(MAX) = CONCAT('SELECT @FbImage = (SELECT * FROM OPENROWSET(BULK ''', @Path, '\devinmajordotcom\Content\HomeImages\reddit.png'', SINGLE_BLOB) AS [Image])');
DECLARE @FacebookImage VARBINARY(MAX);
exec sp_executesql @FacebookImageQuery, N'@FbImage VARBINARY(MAX) OUTPUT', @FbImage=@FacebookImage OUTPUT;

DECLARE @FacebookImageQuery NVARCHAR(MAX) = CONCAT('SELECT @FbImage = (SELECT * FROM OPENROWSET(BULK ''', @Path, '\devinmajordotcom\Content\HomeImages\reddit.png'', SINGLE_BLOB) AS [Image])');
DECLARE @FacebookImage VARBINARY(MAX);
exec sp_executesql @FacebookImageQuery, N'@FbImage VARBINARY(MAX) OUTPUT', @FbImage=@FacebookImage OUTPUT;

DECLARE @FacebookImageQuery NVARCHAR(MAX) = CONCAT('SELECT @FbImage = (SELECT * FROM OPENROWSET(BULK ''', @Path, '\devinmajordotcom\Content\HomeImages\reddit.png'', SINGLE_BLOB) AS [Image])');
DECLARE @FacebookImage VARBINARY(MAX);
exec sp_executesql @FacebookImageQuery, N'@FbImage VARBINARY(MAX) OUTPUT', @FbImage=@FacebookImage OUTPUT;

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
VALUES
(
	@GuestUserID,
	'Facebook',
	NULL,
	NULL,
	'https://www.facebook.com/',
	NULL,
	NULL,
	NULL,
	0,
	1,
	(SELECT @FacebookImage),
	1
),
(
	@GuestUserID,
	'Reddit',
	NULL,
	NULL,
	'https://www.reddit.com/',
	NULL,
	NULL,
	NULL,
	0,
	1,
	(SELECT * FROM OPENROWSET(BULK N'A:\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\HomeImages\reddit.png', SINGLE_BLOB) AS [Image]),
	1
),
(
	@GuestUserID,
	'YouTube',
	NULL,
	NULL,
	'https://www.youtube.com/',
	NULL,
	NULL,
	NULL,
	0,
	1,
	(SELECT * FROM OPENROWSET(BULK N'A:\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\HomeImages\youtube.png', SINGLE_BLOB) AS [Image]),
	1
),
(
	@GuestUserID,
	'Plex Media Dashboard',
	NULL,
	NULL,
	'http://www.devinmajor.com/MediaDashboard',
	NULL,
	NULL,
	NULL,
	0,
	1,
	(SELECT * FROM OPENROWSET(BULK N'A:\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\HomeImages\plex.jpg', SINGLE_BLOB) AS [Image]),
	1
),
(
	@GuestUserID,
	'Slack',
	NULL,
	NULL,
	'https://slack.com/',
	NULL,
	NULL,
	NULL,
	0,
	1,
	(SELECT * FROM OPENROWSET(BULK N'A:\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\HomeImages\slack.png', SINGLE_BLOB) AS [Image]),
	1
),
(
	@GuestUserID,
	'GitHub',
	NULL,
	NULL,
	'https://github.com/',
	NULL,
	NULL,
	NULL,
	0,
	1,
	(SELECT * FROM OPENROWSET(BULK N'A:\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\HomeImages\github.png', SINGLE_BLOB) AS [Image]),
	1
),
(
	@GuestUserID,
	'Newegg',
	NULL,
	NULL,
	'https://www.newegg.ca/',
	NULL,
	NULL,
	NULL,
	0,
	1,
	(SELECT * FROM OPENROWSET(BULK N'A:\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\HomeImages\newegg.jpg', SINGLE_BLOB) AS [Image]),
	1
),
(
	@GuestUserID,
	'Amazon',
	NULL,
	NULL,
	'https://www.amazon.ca/',
	NULL,
	NULL,
	NULL,
	0,
	1,
	(SELECT * FROM OPENROWSET(BULK N'A:\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\HomeImages\amazon2.png', SINGLE_BLOB) AS [Image]),
	1
),
(
	@GuestUserID,
	'Outlook',
	NULL,
	NULL,
	'https://outlook.live.com/owa/',
	NULL,
	NULL,
	NULL,
	0,
	1,
	(SELECT * FROM OPENROWSET(BULK N'A:\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\HomeImages\outlook.jpg', SINGLE_BLOB) AS [Image]),
	1
);
GO

COMMIT