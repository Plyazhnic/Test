CREATE PROCEDURE [UserProfile].[usp_UpdatePersonalInfo]
	@UserProfileID int, 
	@City varchar(50),
	@Address varchar(256),
	@ZipCode varchar(10)
AS
    UPDATE [UserProfile].[UserProfile] 
	SET City = @City,
	[Address] = @Address,
	ZipCode = @ZipCode
    WHERE UserProfileID = @UserProfileID
RETURN 0