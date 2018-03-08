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

DECLARE @Path VARCHAR(MAX) = 'C:\Users\dmajor\Documents\GitHub\devinmajordotcom';
DECLARE @Links TABLE(ID INT IDENTITY(1,1) NOT NULL, [Name] VARCHAR(MAX) NOT NULL, [ImgPathName] VARCHAR(MAX) NOT NULL, [Img] VARBINARY(MAX) NULL, [URL] VARCHAR(MAX) NOT NULL);
DECLARE @Counter1 INT = 1;

INSERT INTO @Links([Name], [ImgPathName], [URL]) 
VALUES 
('Facebook', 'facebook.png', 'https://www.facebook.com'), 
('Reddit', 'reddit.png', 'https://www.reddit.com/'), 
('YouTube', 'youtube.png', 'https://www.youtube.com/'), 
('Slack', 'slack.png', 'https://slack.com/'), 
('GitHub', 'github.png', 'https://github.com/'), 
('Plex Media Dashboard', 'plex.jpg', 'http://www.devinmajor.com/MediaDashboard'), 
('Outlook', 'outlook.jpg', 'https://outlook.live.com/owa/'), 
('Newegg', 'newegg.jpg', 'https://www.newegg.ca/'), 
('Amazon', 'amazon2.png', 'https://www.amazon.ca/');

WHILE(@Counter1 <= (SELECT MAX(ID) FROM @Links))
BEGIN	
	DECLARE @ImageName VARCHAR(MAX) = (SELECT [ImgPathName] FROM @Links WHERE ID = @Counter1);
	DECLARE @ImageQuery NVARCHAR(MAX) = CONCAT('SELECT @img = (SELECT * FROM OPENROWSET(BULK ''', @Path, '\devinmajordotcom\Content\HomeImages\', @ImageName, ''', SINGLE_BLOB) AS [Image])');
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