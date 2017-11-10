use [#dbname]

BEGIN TRANSACTION

INSERT INTO ApplicationMaster([Name])
VALUES
('Main Landing Page'),
('My Custom Homepage'),
('Plex Media Dashboard'),
('Professional Portfolio');
GO

INSERT INTO SiteLinks
(
	DisplayName,
	[Description],
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
	'Index',
	'Portfolio',
	'glyphicon glyphicon-user',
	0,
	1,
	1,
	3
);
GO

COMMIT