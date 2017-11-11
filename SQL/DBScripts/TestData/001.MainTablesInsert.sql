use devinmajordotcom;

BEGIN TRANSACTION

INSERT INTO [User]
(
	ClientName,
	IsActive,
	IsAdmin
)
VALUES
(
	(SELECT HOST_NAME()),
	1,
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
),
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
);
GO

COMMIT