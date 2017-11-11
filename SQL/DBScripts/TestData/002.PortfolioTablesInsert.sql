use devinmajordotcom;

BEGIN TRANSACTION

INSERT INTO [Portfolio].[SkillTypeMaster] ([TypeName])
VALUES('Work/Highlighted'),('Technical'),('Language');
GO

INSERT INTO Portfolio.ProjectType([Type])
VALUES
('Web'),('Desktop'),('Personal'),('Mobile'),('Raspberry Pi');
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
	1994-08-31,
	'58 Algoma Ave, Sault Ste. Marie, ON',
	'+1 (705) 676 - 6783',
	'devinmajor@hotmail.com',
	'Software Developer',
	'I am a professional Programmer and Front-End Developer, capable of creating modern and responsive applications for Desktop, the Web and Mobile. Let''s work together and make something awesome!',
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
	'<p>And just so you know, I am also 21 years old, I have blonde hair and blue eyes, and I''m just over 6 feet tall. I am also a highly motivated and hardworking individual, and I live in Sault Ste. Marie, Ontario, where I''ve recently completed my 3 year degree at Sault College of Applied Arts &amp; Technology, and achieved excellent grades. </p>
	<p>Right now, I am looking for some work, ANY work, in the software industry, and am trying to start a career as a software programmer. Coding is what I love doing in my spare time (what little I have of it), and I love to learn! </p>'
);
GO

INSERT INTO [Portfolio].[Skill]
(
	[Description],
	[DisplayName],
	[ProficiencyPercentage],
	SkillTypeID,
	[DisplayIcon]
)
VALUES
(
	'I create all kinds of programs for businesses and personal use, and I am very particular about my code structure, and user interface design. <br /><b><a href="#portfolio" class="smoothScroll">Want some examples?</a></b>',
	'Desktop Apps',
	NULL,
	1,
	'fa fa-envelope'
),
(
	'I create beautiful, sleek, well built websites for clients that are a breeze to navigate through, yet complex enough to compete with others in a competitive online market.',
	'Dynamic Web Design',
	NULL,
	1,
	'fa fa-flash'
),
(
	'I have a tonne of experience with database programs and design theory, and I am able to create safe, secure spaces to store and manage confidential information. ',
	'Database Design',
	NULL,
	1,
	'fa fa-link'
),
(
	'I design and assemble custom built PCs from scratch. It''s a great way to learn about the inner workings of your machine, and you can tailor to your own needs!',
	'Custom PC Builds',
	NULL,
	1,
	'fa fa-dashboard'
),
(
	'Capable of managing a large scale Local Area Network for your business and home, as well as setting up and maintaining server hardware and software. ',
	'Networking Admin',
	NULL,
	1,
	'fa fa-key'
),
(
	'I have tested and used a number of Windows and Linux operating systems over the years, and have become something of an expert on installing and using them.',
	'Operating Systems',
	NULL,
	1,
	'fa fa-github'
),
(
	'HTML5, CSS, JS',
	'Web',
	85,
	3,
	NULL
),
(
	'Java, Android/iOS, C++',
	'Object Oriented, Cross Platform',
	75,
	3,
	NULL
),
(
	'SQL, MySql, PHP',
	'Databases & Scripting',
	90,
	3,
	NULL
),
(
	'Visual Basic, C#, F#',
	'.NET',
	100,
	3,
	NULL
),
(
	'Scripts',
	'Python',
	80,
	3,
	NULL
),
(
	'Can propose and justify the design and development of an integrated software solution based on an analysis of the business environment',
	NULL,
	NULL,
	2,
	NULL
),
(
	'Very familiar with Version Control Systems such as CVS, Git, and Mercurial',
	NULL,
	NULL,
	2,
	NULL
),
(
	'Knowledge of security issues in the analysis, design, and implementation of software',
	NULL,
	NULL,
	2,
	NULL
),
(
	'Knowledge of the design, modeling, implementation, and maintenance of a database',
	NULL,
	NULL,
	2,
	NULL
),
(
	'Can design, test, document, and deploy programs based on specifications',
	NULL,
	NULL,
	2,
	NULL
),
(
	'Excellent at developing and maintaining effective working relationships with clients',
	NULL,
	NULL,
	2,
	NULL
),
(
	'Knowledge of networking concepts to develop, deploy, and maintain a working LAN based on users needs',
	NULL,
	NULL,
	2,
	NULL
),
(
	'Extremely proficient in Microsoft Office Programs and troubleshooting hardware/software',
	NULL,
	NULL,
	2,
	NULL
),
(
	'Can analyze and define the specifications of a system based on requirements.',
	NULL,
	NULL,
	2,
	NULL
);
GO

INSERT INTO Portfolio.Project
(
[Name],
[Description],
[Image]
)
VALUES
(
'UX Design',
'Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonumm.',
(SELECT * FROM OPENROWSET(BULK N'C:\Users\Devin Major\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\PortfolioImages\winIOT.jpg', SINGLE_BLOB) AS [Image])
),
(
'UX Design',
'Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonumm.',
(SELECT * FROM OPENROWSET(BULK N'C:\Users\Devin Major\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\PortfolioImages\boggle.jpg', SINGLE_BLOB) AS [Image])
),
(
'UX Design',
'Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonumm.',
(SELECT * FROM OPENROWSET(BULK N'C:\Users\Devin Major\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\PortfolioImages\spwha.png', SINGLE_BLOB) AS [Image])
),
(
'UX Design',
'Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonumm.',
(SELECT * FROM OPENROWSET(BULK N'C:\Users\Devin Major\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\PortfolioImages\hangman.jpg', SINGLE_BLOB) AS [Image])
),
(
'UX Design',
'Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonumm.',
(SELECT * FROM OPENROWSET(BULK N'C:\Users\Devin Major\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\PortfolioImages\bugbyte.png', SINGLE_BLOB) AS [Image])
),
(
'UX Design',
'Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonumm.',
(SELECT * FROM OPENROWSET(BULK N'C:\Users\Devin Major\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\PortfolioImages\bag.jpg', SINGLE_BLOB) AS [Image])
),
(
'UX Design',
'Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonumm.',
(SELECT * FROM OPENROWSET(BULK N'C:\Users\Devin Major\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\PortfolioImages\portfolio-img7.jpg', SINGLE_BLOB) AS [Image])
),
(
'UX Design',
'Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonumm.',
(SELECT * FROM OPENROWSET(BULK N'C:\Users\Devin Major\Documents\GitHub\devinmajordotcom\devinmajordotcom\Content\PortfolioImages\portfolio-img8.jpg', SINGLE_BLOB) AS [Image])
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
(2,3),
(2,4),
(2,5),
(3,3),
(4,1),
(4,4),
(5,3),
(6,1),
(6,2),
(7,2),
(8,3);
GO

COMMIT