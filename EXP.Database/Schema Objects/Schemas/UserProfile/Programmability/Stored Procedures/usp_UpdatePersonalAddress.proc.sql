CREATE PROCEDURE [UserProfile].[usp_UpdatePersonalAddress]
	@UserProfileID int, 
	@AddressID int
AS
    UPDATE [UserProfile].[UserProfile] 
	SET AddressID = @AddressID
    WHERE UserProfileID = @UserProfileID
RETURN 0