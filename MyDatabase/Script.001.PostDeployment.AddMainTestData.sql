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

INSERT INTO [Security].[HardwareType](Name)
VALUES('CPU 1 Usage'),('RAM Usage'),('CPU 1 Temp');

INSERT INTO [Security].[DateRange](Name)
VALUES('Last 60 Seconds'),('Last 60 Minutes'),('Last 24 Hours'), ('Last 7 Days'), ('Last 30 Days');

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

INSERT INTO [LandingPage].[Config]
(
	AppsTitle,
	IsParticleCanvasOn,
	BackgroundImage,
	WebsiteName
)
VALUES
(
	'Welcome, Fellow Humans!',
	1,
	'~/Content/Images/back3.png',
	'D3V!N M@J0R'
);
GO

INSERT INTO [LandingPage].[SiteLink]
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
	'My Custom Homepage', 
	'My user-based, blog style homepage! Click to see my home, or to login to yours.',
	NULL,
	NULL,
	'Index',
	'MyHome',
	'glyphicon-dashboard',
	0,
	1,
	1,
	1
),
(
	'Plex Media Dashboard', 
	'Movies, TV, Music, Photos and More! Grab a drink and click here to be entertained.',
	NULL,
	NULL,
	'Index',
	'MediaDashboard',
	'glyphicon-film',
	0,
	1,
	1,
	2
),
(
	'Professional Portfolio', 
	'Gives visitors some insight into who I am, what I do, and some of my past projects.',
	NULL,
	NULL,
	'Index',
	'Portfolio',
	'glyphicon-user',
	0,
	1,
	1,
	3
);

INSERT INTO [Security].[EmailType]
(
	[TypeName]
)
VALUES ('Email Confirmation'), ('Password Reset'), ('Update Credentials'), ('Other');

INSERT INTO [LandingPage].[BannerLink]
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
	'Home & Apps', 
	'You''ve reached the coolest corner of the Internet!',
	'Feel free to click one of the applications below for more fun... you never know what you''ll find!',
	'#home',
	'Index',
	'MyHome',
	'fa-home',
	1,
	1,
	1,
	1
),
(
	'Server Status', 
	'This Website is Brought To You By...',
	'Watch the live feed of statistics below, and marvel at the beauty of my server.',
	'#server',
	'Index',
	'MediaDashboard',
	'fa-heartbeat',
	0,
	1,
	1,
	2
),
(
	'Contact Me', 
	'Drop Me a Line',
	'If you''d like excusive access to any of the services here (Plex, Custom Home, etc.), then feel free to send me a quick message, and I''ll take it from there!',
	'#contact',
	'Index',
	'Portfolio',
	'fa-envelope',
	0,
	1,
	1,
	3
);

COMMIT