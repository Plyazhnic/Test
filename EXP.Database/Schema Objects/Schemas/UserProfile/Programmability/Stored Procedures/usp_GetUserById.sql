CREATE PROCEDURE [UserProfile].[usp_GetUserById]
	@UserId int
AS
		SELECT * From [UserProfile].[UserProfile] WHERE UserProfileID = @UserId

RETURN 0