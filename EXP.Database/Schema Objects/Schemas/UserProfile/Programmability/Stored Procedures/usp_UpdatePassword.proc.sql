CREATE PROCEDURE [UserProfile].[usp_UpdatePassword]
    @UserName nchar(10),
    @Password varchar(1024),
    @Salt varchar(1024)
AS
    UPDATE [UserProfile].[UserProfile]
    SET ProfilePassword = @Password, ProfilePasswordSalt = @Salt
    WHERE LoginName = @UserName

RETURN 0