CREATE PROCEDURE [UserProfile].[usp_SetStatus]
	@UserProfileId int, 
	@StatusId int
AS
	UPDATE [UserProfile].[UserProfile] SET UserProfileStatusID = @StatusId
	WHERE UserProfileID = @UserProfileId
RETURN 0