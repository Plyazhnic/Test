CREATE PROCEDURE [UserProfile].[usp_CreateUser]
	@UserProfileTypeID int,
	@UserProfileStatusID tinyint,
	@UserProfileEncryptionTypeID int, 
	@LoginName char(10), 
	@ProfilePassword varchar(1024), 
	@ProfilePasswordSalt varchar(1024), 
	@EmailAddress varchar(256),
	@TitlePreffix char(4),
	@TitleSuffix char(4),
	@FirstName varchar(256),
	@LastName varchar(256),
	@Comments varchar(1024),
	@AgreementVersion varchar(50),
	@AgreementDate datetime
	
AS
	INSERT INTO [UserProfile].[UserProfile]
           ([UserProfileTypeID]
           ,[UserProfileStatusID]
		   ,[UserProfileEncryptionTypeID]
           ,[LoginName]
           ,[ProfilePassword]
		   ,[ProfilePasswordSalt]
           ,[EmailAddress]
           ,[TitlePreffix]
           ,[TitleSuffix]
           ,[FirstName]
           ,[LastName]
           ,[Comments]
           ,[AgreementVersion]
           ,[AgreementDate]
           ,[LastLoginDate]
           ,[UpdateDate]
           ,[CreateDate])
     VALUES
           (@UserProfileTypeID,
           @UserProfileStatusID,
		   @UserProfileEncryptionTypeID,
           @LoginName,
           @ProfilePassword, 
		   @ProfilePasswordSalt,
           @EmailAddress, 
           @TitlePreffix,
           @TitleSuffix,
           @FirstName, 
           @LastName, 
           @Comments, 
           @AgreementVersion,
           @AgreementDate,
           GETDATE(),
           GETDATE(),
           GETDATE())

RETURN 0