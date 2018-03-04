CREATE TABLE [MyHome].[BlogPostComment]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	UserID INT NOT NULL,
	CommentBody VARCHAR(MAX) NOT NULL,
	PRIMARY KEY(Id),
	CONSTRAINT MyHome_BlogPostComment_UserID_Security_User_Id
	FOREIGN KEY(UserID) REFERENCES [Security].[User](ID)
)
