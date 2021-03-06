﻿/*
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

--TODO: REPLACE ALL LINKS BELOW WITH BETTER TEST DATA

INSERT INTO [MediaDashboard].[SiteLink]
(
	DisplayName,
	[Description],
	[Directive],
	[URL],
	[Action],
	[Controller],
	DisplayIcon,
	IsDefault,
	IsEnabled,
	IsPublic,
	[Order]
)
VALUES
(
	'Plex',
	NULL,
	NULL,
	'https://www.howtogeek.com/252261/how-to-set-up-plex-and-watch-your-movies-on-any-device/',
	NULL,
	NULL,
	'fa-dashboard',
	1,
	1,
	1,
	1
),
(
	'Requests',
	NULL,
	NULL,
	'https://ombi.io/',
	NULL,
	NULL,
	'fa-search',
	0,
	1,
	1,
	2
),
(
	'CouchPotato',
	NULL,
	NULL,
	'https://couchpota.to/',
	NULL,
	NULL,
	'fa-film',
	0,
	1,
	0,
	3
),
(
	'Sonarr',
	NULL,
	NULL,
	'https://sonarr.tv/',
	NULL,
	NULL,
	'fa-desktop',
	0,
	1,
	0,
	4
),
(
	'Headphones',
	NULL,
	NULL,
	'https://www.cuttingcords.com/home/ultimate-server/headphones',
	NULL,
	NULL,
	'fa-headphones',
	0,
	1,
	0,
	5
),
(
	'Deluge',
	NULL,
	NULL,
	'http://deluge-torrent.org/',
	NULL,
	NULL,
	'fa-database',
	0,
	1,
	0,
	6
),
(
	'Tautulli',
	NULL,
	NULL,
	'http://tautulli.com/',
	NULL,
	NULL,
	'fa-play',
	0,
	1,
	0,
	7
),
(
	'Jackett',
	NULL,
	NULL,
	'http://tricksty.com/tricks/sonarr-how-to-add-good-public-indexers',
	NULL,
	NULL,
	'fa-bullhorn',
	0,
	1,
	0,
	8
),
(
	'KodExplorer',
	NULL,
	NULL,
	'http://demo.kodcloud.com/index.php?user/login&link=http%3A%2F%2Fdemo.kodcloud.com%2F',
	NULL,
	NULL,
	'fa-hdd-o',
	0,
	1,
	0,
	9
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
	,WebsiteTitle
)
VALUES
(
	@GuestUserID,
	'My Media Dashboard',
	'M D',
	'black',
	'#28a08c',
	'D3V!N M@J0R'
);
GO

COMMIT