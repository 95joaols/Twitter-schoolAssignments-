USE OOPGroup1;

DROP TABLE IF EXISTS PrivateMessage;
DROP TABLE IF EXISTS UserToRetweet;
DROP TABLE IF EXISTS Tweet;
DROP TABLE IF EXISTS UserToUser; 
DROP TABLE IF EXISTS [User];
------------

CREATE TABLE [User] (Id int IDENTITY(1,1) PRIMARY KEY, Username varchar(50) NOT NULL UNIQUE, Firstname varchar(50), Lastname varchar(50), [Password] varchar(MAX) NOT NULL, Biography varchar(200), IsLoggedIn bit DEFAULT 0, PasswordSalt varchar(MAX) NOT NULL);
CREATE TABLE Tweet (Id int IDENTITY(1,1) PRIMARY KEY, CreateDate datetime DEFAULT GETDATE(), UserId int NOT NULL, [Message] varchar(100) NOT NULL);
CREATE TABLE UserToUser (Id int IDENTITY(1,1) PRIMARY KEY, UserId int NOT NULL, FollowingId int NOT NULL);
CREATE TABLE UserToRetweet (Id int IDENTITY(1,1) PRIMARY KEY, UserId int NOT NULL, TweetId int NOT NULL);
CREATE TABLE PrivateMessage (Id int IDENTITY(1,1) PRIMARY KEY, UserFromId int NOT NULL, UserToId int NOT NULL, [Message] varchar(100) NOT NULL);

ALTER TABLE PrivateMessage ADD FOREIGN KEY (UserFromId) REFERENCES [User](Id);
ALTER TABLE PrivateMessage ADD FOREIGN KEY (UserToId) REFERENCES [User](Id);
ALTER TABLE Tweet ADD FOREIGN KEY (UserId) REFERENCES [User](Id);
ALTER TABLE UserToUser ADD FOREIGN KEY (UserId) REFERENCES [User](Id);
ALTER TABLE UserToUser ADD FOREIGN KEY (FollowingId) REFERENCES [User](Id);
ALTER TABLE UserToRetweet ADD FOREIGN KEY (UserId) REFERENCES [User](Id);
ALTER TABLE UserToRetweet ADD FOREIGN KEY (TweetId) REFERENCES [Tweet](Id) ON DELETE CASCADE;

INSERT INTO [User] (Username, [Password], Biography, PasswordSalt) VALUES ('a', 'GsYOlbZ9mwt2GJUy3u8WY0nmB12QTEwioMxm99CsMQOQ0zcdFF6e/KbtY3ddrw28+MFTTzJDd0EgZoYfY3R/7KR6cW0ZxYZtQnHLLCJPCz7yypd2x0GK3DD1IAd3x9vsKUKYWxqPKOpgoDNOd3qsC1+Aiw6d5uKlZMpG3pRqvco=', 'Gotta go fast!', '$2b$10$.j3ycwtbr8Agm11hrwG9Ue');

-------------------
SELECT * FROM [User];
SELECT * FROM Tweet;
SELECT * FROM UserToUser;
SELECT * FROM UserToRetweet;
SELECT * FROM PrivateMessage;

-------------------
DROP VIEW IF EXISTS UsersView;
DROP VIEW IF EXISTS TweetsView;
DROP PROC IF EXISTS SearchProcedureUsers;
DROP PROC IF EXISTS SearchProcedureTweets;

------------

CREATE OR ALTER VIEW UsersView AS
SELECT TOP 50 [User].Id, [User].Username, [User].Firstname, [User].Lastname, [User].Biography
FROM [User];

------------

CREATE OR ALTER VIEW TweetsView AS
SELECT TOP 50 Tweet.Id, Tweet.[Message], Tweet.CreateDate, [User].Id AS UserId, [User].Username		-- Kan behöva hämta RetweetId här också.
FROM Tweet INNER JOIN [User] ON [Tweet].UserId = [User].Id ORDER BY CreateDate DESC;			-- Prolem med INNER JOIN: Om man inte skrivit någon Tweet (i tabellen Tweet!) så kommer inte det Username:t att finnas med.

------------

CREATE OR ALTER PROCEDURE SearchProcedureUsers
@SearchString varchar(100) = NULL,
@IdToExclude int = NULL AS
BEGIN
	IF @IdToExclude = 0
	BEGIN
		SELECT * FROM UsersView
		WHERE Username LIKE '%' + @SearchString + '%'
		OR Firstname LIKE '%' + @SearchString + '%'
		OR Lastname LIKE '%' + @SearchString + '%'
		OR Biography LIKE '%' + @SearchString + '%'
	END
	IF @IdToExclude <> 0
	BEGIN
		SELECT * FROM UsersView
		WHERE Username LIKE '%' + @SearchString + '%' AND Id <> @IdToExclude
		OR Firstname LIKE '%' + @SearchString + '%' AND Id <> @IdToExclude
		OR Lastname LIKE '%' + @SearchString + '%' AND Id <> @IdToExclude
		OR Biography LIKE '%' + @SearchString + '%' AND Id <> @IdToExclude
	END
END

------------
CREATE OR ALTER PROCEDURE SearchProcedureTweets
@SearchString varchar(100) = NULL AS
BEGIN
	SELECT * FROM TweetsView
	WHERE [Message] LIKE '%' + @SearchString + '%'
END