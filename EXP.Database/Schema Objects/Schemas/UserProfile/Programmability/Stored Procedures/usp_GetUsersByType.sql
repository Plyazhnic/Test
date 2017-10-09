CREATE PROCEDURE [UserProfile].[usp_GetUsersByType]
	@UserProfileTypeID int 
AS
	SELECT * FROM [UserProfile].[UserProfile] WHERE UserProfileTypeID = @UserProfileTypeID
RETURN 0
