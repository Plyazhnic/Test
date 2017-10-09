CREATE PROCEDURE [UserProfile].[usp_ActivateUser]
	@Email varchar(256),
	@Salt varchar(1024)
AS
	UPDATE [UserProfile].[UserProfile] SET UserProfileStatusID = 5
	WHERE dbo.TRIM(EmailAddress) = @Email AND ProfilePasswordSalt = @Salt AND UserProfileStatusID = 4

RETURN 0