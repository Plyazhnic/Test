CREATE PROCEDURE [dbo].[usp_GetPhonesForUser]
	@UserProfileID int 
AS
    SELECT TOP 2 Phone.*, PhoneType.PhoneType 
	FROM [dbo].[Phone] 
    INNER JOIN dbo.PhoneType ON dbo.Phone.PhoneTypeID = dbo.PhoneType.PhoneTypeID
	INNER JOIN UserProfile.PhonesToProfiles ON dbo.Phone.PhoneID = UserProfile.PhonesToProfiles.PhoneID
	 WHERE  PhonesToProfiles.UserProfileID=@UserProfileID ORDER BY Phone.CreatedDate DESC

RETURN 0