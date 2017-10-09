CREATE PROCEDURE [dbo].[usp_GetAllPhonesForUser]
	@UserProfileID int 
AS
    SELECT  Phone.*, PhoneType.PhoneType 
	FROM [dbo].[Phone] 
    INNER JOIN dbo.PhoneType ON dbo.Phone.PhoneTypeID = dbo.PhoneType.PhoneTypeID
	INNER JOIN UserProfile.PhonesToProfiles ON dbo.Phone.PhoneID = UserProfile.PhonesToProfiles.PhoneID
	 WHERE  PhonesToProfiles.UserProfileID=@UserProfileID ORDER BY Phone.CreatedDate 

RETURN 0