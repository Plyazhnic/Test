CREATE PROCEDURE [dbo].[usp_GetCompanyForUser]
	@UserProfileID int
AS
	SELECT [dbo].[Company].*, UserProfile.[Address].*, Work.PhoneID, Work.PhoneTypeID, Work.PhoneNumber,
		Fax.PhoneID AS FaxPhoneID, Fax.PhoneTypeID AS FaxPhoneTypeID, Fax.PhoneNumber AS FaxPhoneNumber FROM [dbo].[Company]
		INNER JOIN dbo.CompanyToProfiles ON [dbo].[Company].CompanyID = dbo.CompanyToProfiles.CompanyID
		LEFT JOIN UserProfile.[Address] ON [dbo].[Company].AddressID = UserProfile.[Address].AddressID
		LEFT JOIN dbo.Phone AS Work ON [dbo].[Company].WorkPhoneID = Work.PhoneID
		LEFT JOIN dbo.Phone AS Fax ON [dbo].[Company].FaxID = Fax.PhoneID
	WHERE UserProfileID = @UserProfileID

RETURN 0
