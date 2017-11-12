use [#dbname];

BEGIN TRANSACTION

INSERT INTO [dbo].[User]
(
	ClientName,
	IsActive,
	EmailAddress,
	IsAdmin
)
VALUES
(
	(SELECT HOST_NAME()),
	1,
	'devinmajor@hotmail.com',
	1
);
GO

INSERT INTO ApplicationMaster([Name])
VALUES
('Main Landing Page'),
('My Custom Homepage'),
('Plex Media Dashboard'),
('Professional Portfolio');
GO

--TODO: REPLACE ALL LINKS BELOW WITH BETTER TEST DATA

INSERT INTO SiteLink
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
	ApplicationID,
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
	'glyphicon glyphicon-dashboard',
	0,
	1,
	1,
	1
),
(
	'Plex Media Dashboard', 
	'Movies, TV, Music, Photos and More! Click here to be entertained.',
	NULL,
	NULL,
	'Index',
	'MediaDashboard',
	'glyphicon glyphicon-film',
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
	'glyphicon glyphicon-user',
	0,
	1,
	1,
	3
),

(
	'Plex',
	NULL,
	NULL,
	'http://70.76.54.169:32400/web/index.html',
	NULL,
	NULL,
	'home',
	1,
	1,
	3,
	1
),
(
	'Requests',
	NULL,
	NULL,
	'http://70.76.54.169:3579',
	NULL,
	NULL,
	'search',
	0,
	1,
	3,
	1
),
(
	'CouchPotato',
	NULL,
	NULL,
	'http://70.76.54.169:5050',
	NULL,
	NULL,
	'pencil',
	0,
	1,
	3,
	1
),
(
	'Sonarr',
	NULL,
	NULL,
	'http://70.76.54.169:8989',
	NULL,
	NULL,
	'about',
	0,
	1,
	3,
	1
),
(
	'Headphones',
	NULL,
	NULL,
	'http://70.76.54.169:7171/home',
	NULL,
	NULL,
	'archive',
	0,
	1,
	3,
	1
),
(
	'Deluge',
	NULL,
	NULL,
	'http://70.76.54.169:8112',
	NULL,
	NULL,
	'contact',
	0,
	1,
	3,
	1
),

(
	'Facebook',
	NULL,
	'Follow me on',
	'#',
	NULL,
	NULL,
	'fa fa-facebook',
	0,
	1,
	4,
	1
),
(
	'Reddit',
	NULL,
	'Find me on',
	'#',
	NULL,
	NULL,
	'fa fa-reddit',
	0,
	1,
	4,
	2
),
(
	'GitHub',
	NULL,
	'Fork me on',
	'#',
	NULL,
	NULL,
	'fa fa-github',
	0,
	1,
	4,
	3
);
GO

COMMIT