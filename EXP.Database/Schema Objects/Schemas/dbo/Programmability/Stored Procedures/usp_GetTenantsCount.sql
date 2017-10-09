CREATE PROCEDURE [dbo].[usp_GetTenantsCount]
	@Search varchar(50),
	@BuildingId int
AS
	SELECT @Search = '%' + RTRIM(@Search) + '%';

	SELECT count(*)
	
FROM [dbo].[BuildingToCompanies] 
     INNER JOIN [dbo].[Company] ON [dbo].[BuildingToCompanies].CompanyID = [dbo].[Company].CompanyID
	 INNER JOIN [UserProfile].[Address] ON [dbo].[Company].[AddressID] = [UserProfile].[Address].AddressID
     INNER JOIN [dbo].[CompanyToProfiles] ON dbo.CompanyToProfiles.CompanyID = [dbo].[Company].CompanyID
     INNER JOIN [UserProfile].[UserProfile] ON [dbo].[CompanyToProfiles].UserProfileID = [UserProfile].[UserProfile].UserProfileID
WHERE  [dbo].[BuildingToCompanies].BuildingID = @BuildingId AND [UserProfile].[UserProfile].UserProfileTypeID = 4 AND [dbo].[Company].CompanyName LIKE @Search 

RETURN 0
