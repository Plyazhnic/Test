CREATE PROCEDURE [UserProfile].[usp_ListUserProfileType]
	AS
	SELECT UserProfileTypeId, UserProfileType FROM UserProfile.UserProfileType WHERE isActive = 1 AND UserProfileTypeId > 2
RETURN 0