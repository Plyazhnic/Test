CREATE PROCEDURE [UserProfile].[usp_GetUserByEmail]
	@Email varchar(256),
	@UserName varchar(256) OUTPUT
AS
	SELECT @UserName = LoginName 
	FROM [UserProfile].[UserProfile]
	WHERE EmailAddress = @Email

RETURN 0