CREATE PROCEDURE [UserProfile].[usp_GetUser]
	@UserName char(10),
    @Status tinyint
AS
	IF @Status = 0 
	BEGIN
		SELECT * From [UserProfile].[UserProfile] WHERE dbo.TRIM(LoginName) = @UserName
	END
	ELSE
		SELECT * From [UserProfile].[UserProfile] WHERE dbo.TRIM(LoginName) = @UserName AND UserProfileStatusID = @Status

RETURN 0