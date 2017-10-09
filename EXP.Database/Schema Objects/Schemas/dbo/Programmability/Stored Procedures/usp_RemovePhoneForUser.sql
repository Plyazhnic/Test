CREATE PROCEDURE [dbo].[usp_RemovePhoneForUser]
	@PhoneID int,
	@UserProfileID int
AS
	DELETE FROM [UserProfile].[PhonesToProfiles]
	WHERE PhoneID=@PhoneID AND UserProfileID=@UserProfileID
RETURN 0
