CREATE PROCEDURE [UserProfile].[usp_SearchSessionId]
	@SessionID varchar(32)
AS
	SELECT SessionID FROM UserProfile.UserProfile
	WHERE SessionID = @SessionID
RETURN 0
