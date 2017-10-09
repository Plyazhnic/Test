CREATE PROCEDURE [dbo].[usp_GetPhoneForUserByType]
	@UserProfileID int, 
	@PhoneTypeID int 

AS
    SELECT TOP 1 Phone.*, PhoneType.PhoneType 
	FROM [dbo].[Phone] 
    INNER JOIN dbo.PhoneType ON dbo.Phone.PhoneTypeID = dbo.PhoneType.PhoneTypeID
	INNER JOIN UserProfile.PhonesToProfiles ON dbo.Phone.PhoneID = UserProfile.PhonesToProfiles.PhoneID
	 WHERE  PhonesToProfiles.UserProfileID=@UserProfileID AND Phone.PhoneTypeID = @PhoneTypeID ORDER BY Phone.CreatedDate DESC

RETURN 0