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

INSERT INTO [Portfolio].[ContactLink]
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
	[Order],
	Color
)
VALUES
(
	'Facebook',
	NULL,
	'Follow me on',
	'https://www.facebook.com/iWillHackIntoYourSoul/',
	NULL,
	NULL,
	'fa-facebook-f',
	0,
	1,
	1,
	'#3b5998'
),
(
	'LinkedIn',
	NULL,
	'Find me on',
	'https://www.linkedin.com/in/devin-major-782ba08b/',
	NULL,
	NULL,
	'fa-linkedin',
	0,
	1,
	2,
	'#0099CC'
),
(
	'GitHub',
	NULL,
	'Fork me on',
	'https://github.com/bug-Byte/',
	NULL,
	NULL,
	'fa-github',
	0,
	1,
	3,
	'#777777'
);
GO

INSERT INTO Portfolio.ProjectType([Type])
VALUES
('Web'),('Desktop'),('Personal'),('Mobile'),('Raspberry Pi');
GO

INSERT INTO Portfolio.Config(WebsiteTitle,BackgroundImage)
VALUES
(
	'D3V!N M@J0R',
	'~/Content/PortfolioImages/back1.png'
);
GO

INSERT INTO [Portfolio].[Profile]
(
	[FirstName]
	,[LastName]
	,[DateOfBirth]
	,[Address]
	,[PhoneNumber]
	,[EmailAddress]
	,[PositionTitle]
	,[PersonalDescription]
	,[WebsiteText]
	,[WebsiteURL]
)
VALUES
(
	'Devin',
	'Major',
	'19940831 12:00:00 AM',
	'58 Algoma Ave, Sault Ste. Marie, ON',
	'+1 (705) 676 - 6783',
	'devinmajor@hotmail.com',
	'Software Developer',
	'I am a professional Programmer and Full-Stack Developer, capable of creating modern and responsive applications for Desktop, the Web and Mobile. Let''s work together and make something awesome!',
	'YOU''RE HERE ALREADY!',
	'#home'
);
GO

INSERT INTO Portfolio.PersonalDescription
(
	Adjective1,
	Adjective2,
	Adjective3,
	Blurb
)
VALUES
(
	'Creative',
	'Fun',
	'Awesome',
	'<p>And just so you know, I am also 24 years old, I have blonde hair and blue eyes, and I''m just over 6 feet tall. I am also a highly motivated and hardworking individual, and I live in Sault Ste. Marie, Ontario, where I''ve recently completed my 3 year degree at Sault College of Applied Arts &amp; Technology, and achieved excellent grades. </p>
	<p>Right now, I am looking for some work, ANY work, in the software industry, and am trying to start a career as a software developer. Coding is what I love doing in my spare time (what little I have of it), and I love to learn! </p>'
);
GO

INSERT INTO [Portfolio].[HighlightedSkill]
(
	[Description],
	[DisplayName],
	[ProficiencyPercentage],
	[DisplayIcon]
)
VALUES
(
	'I create all kinds of programs for businesses and personal use, and I am very particular about my code structure, and user interface design. <br /><b><a href="#portfolio" class="smoothScroll">Want some examples?</a></b>',
	'Desktop Apps',
	NULL,
	'fa-envelope'
),
(
	'I create beautiful, sleek, well built websites for clients that are a breeze to navigate through, yet complex enough to compete with others in a competitive online market.',
	'Dynamic Web Design',
	NULL,
	'fa-flash'
),
(
	'I have a tonne of experience with database programs and design theory, and I am able to create safe, secure spaces to store and manage confidential information. ',
	'Database Design',
	NULL,
	'fa-link'
),
(
	'I design and assemble custom built PCs from scratch. It''s a great way to learn about the inner workings of your machine, and you can tailor to your own needs!',
	'Custom PC Builds',
	NULL,
	'fa-dashboard'
),
(
	'Capable of managing a large scale Local Area Network for your business and home, as well as setting up and maintaining server hardware and software. ',
	'Networking Admin',
	NULL,
	'fa-key'
),
(
	'I have tested and used a number of Windows and Linux operating systems over the years, and have become something of an expert on installing and using them.',
	'Operating Systems',
	NULL,
	'fa-github'
);
GO

INSERT INTO [Portfolio].[LanguageSkill]
(
	[Description],
	[DisplayName],
	[ProficiencyPercentage],
	[DisplayIcon]
)
VALUES
(
	'HTML5, CSS, JS',
	'Web',
	85,
	NULL
),
(
	'Java, Android/iOS, C++',
	'Object Oriented, Cross Platform',
	75,
	NULL
),
(
	'SQL, MySql, PHP',
	'Databases & Scripting',
	90,
	NULL
),
(
	'Visual Basic, C#, F#',
	'.NET',
	100,
	NULL
),
(
	'Scripts',
	'Python',
	80,
	NULL
);
GO

INSERT INTO [Portfolio].[TechSkill]
(
	[Description],
	[DisplayName],
	[ProficiencyPercentage],
	[DisplayIcon]
)
VALUES
(
	'Can propose and justify the design and development of an integrated software solution based on an analysis of the business environment',
	NULL,
	NULL,
	NULL
),
(
	'Very familiar with Version Control Systems such as CVS, Git, and Mercurial',
	NULL,
	NULL,
	NULL
),
(
	'Knowledge of security issues in the analysis, design, and implementation of software',
	NULL,
	NULL,
	NULL
),
(
	'Knowledge of the design, modeling, implementation, and maintenance of a database',
	NULL,
	NULL,
	NULL
),
(
	'Can design, test, document, and deploy programs based on specifications',
	NULL,
	NULL,
	NULL
),
(
	'Excellent at developing and maintaining effective working relationships with clients',
	NULL,
	NULL,
	NULL
),
(
	'Knowledge of networking concepts to develop, deploy, and maintain a working LAN based on users needs',
	NULL,
	NULL,
	NULL
),
(
	'Extremely proficient in Microsoft Office Programs and troubleshooting hardware/software',
	NULL,
	NULL,
	NULL
),
(
	'Can analyze and define the specifications of a system based on requirements.',
	NULL,
	NULL,
	NULL
);
GO

DECLARE @Projects TABLE(ID INT IDENTITY(1,1) NOT NULL, [Name] VARCHAR(MAX) NOT NULL, [ImgPathName] VARCHAR(MAX) NOT NULL, [Img] VARBINARY(MAX) NULL, [Description] VARCHAR(MAX) NOT NULL);

INSERT INTO @Projects([Name], [ImgPathName], [Description]) 
VALUES 
('devinmajor.com', '~/Content/PortfolioImages/devinmajordotcom.png', 'Created and hosted full stack, responsive, functional, and totally cool looking website for the world to see! Written in C# .NET.'), 
('Plex Media Dashboard', '~/Content/PortfolioImages/mediadashboard.png', 'Created and maintain a Plex Media Server, with many other plugins like Ombi for requests, Tautulli for monitoring, etc.'), 
('MyHome', '~/Content/PortfolioImages/myhome.png', 'Created a user-based custom homepage-style website, featuring a blog, weather, date & time, favorites & more!'), 
('My Portfolio', '~/Content/PortfolioImages/bugbyte.png', 'You''re here already! This is an excellent example of a project I''ve completed that you can see!'), 
('MIS - Scorecards', '~/Content/PortfolioImages/scorecard.png', 'Created an employee performance tracking module, featuring CRUD implementation, AJAX, KPI Management and more.'), 
('MIS - Target Sheets', '~/Content/PortfolioImages/targetsheets.png', 'Created a KPI target & score scale management module, featuring bootstrap layouts and jQuery DataTables.'), 
('VMS Public Form', '~/Content/PortfolioImages/vmsform.png', 'Created a public facing registration form for buyers of a company, featuring ReCaptcha, AngularJS and Handlebars.'), 
('VMS Intranet', '~/Content/PortfolioImages/vmsadmin.png', 'Created an interal facing website for managing buyer registrations from the public form.'), 
('Windows IoT App', '~/Content/PortfolioImages/winIOT.jpg', 'Created a small Windows ARM program that allowed web browsing, displays weather, and other basic functionalities.'), 
('Boggle', '~/Content/PortfolioImages/boggle.jpg', 'Created a fully featured Boggle game in VB .NET.'), 
('Soo PeeWee Hockey', '~/Content/PortfolioImages/spwha.png', 'Created a fully responsive PHP based website for the Sault PeeWee Hockey Association.'), 
('PiBag', '~/Content/PortfolioImages/bag.jpg', 'I''ve created a custom laptop bag that has a Raspberry Pi and LCD touch display build into it! Running Arch Linux.');

INSERT INTO Portfolio.Project
(
	[Name],
	[Description],
	[Image]
)
(
	SELECT 
		t.Name,
		t.[Description],
		t.[ImgPathName]
	FROM @Projects t
);
GO

INSERT INTO Portfolio.ProjectTypeMapping(ProjectID, ProjectTypeID)
VALUES
(1,1),
(1,2),
(1,3),
(1,4),
(1,5),
(2,1),
(2,2),
(2,3),
(2,4),
(3,1),
(3,2),
(3,4),
(4,1),
(4,2),
(4,4),
(5,1),
(5,3),
(5,4),
(6,1),
(6,3),
(6,4),
(7,1),
(7,3),
(7,4),
(8,1),
(8,3),
(8,4),
(9,1),
(9,3),
(9,4),
(9,5),
(10,2),
(10,3),
(11,1),
(11,2),
(11,4),
(12,2),
(12,3),
(12,4),
(12,5);
GO

COMMIT