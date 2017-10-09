CREATE PROCEDURE [dbo].[usp_ListTenantsForPage]
	@Start int ,
	@End int,
	@Search varchar(50),
	@BuildingId int
AS

SELECT @Search = '%' + RTRIM(@Search) + '%';

WITH TenantPage AS
( 
SELECT   [dbo].[Company].CompanyID, [dbo].[Company].CompanyName, [UserProfile].[Address].City, [UserProfile].[Address].Address1, [UserProfile].[Address].Address2, [UserProfile].[UserProfile].FirstName, [UserProfile].[UserProfile].LastName,[UserProfile].[UserProfile].EmailAddress,
ROW_NUMBER() OVER (ORDER BY [dbo].[Company].CompanyID ) AS RowNumber
FROM [dbo].[BuildingToCompanies] 
     INNER JOIN [dbo].[Company] ON [dbo].[BuildingToCompanies].CompanyID = [dbo].[Company].CompanyID
	 INNER JOIN [UserProfile].[Address] ON [dbo].[Company].[AddressID] = [UserProfile].[Address].AddressID
     INNER JOIN [dbo].[CompanyToProfiles] ON dbo.CompanyToProfiles.CompanyID = [dbo].[Company].CompanyID
     INNER JOIN [UserProfile].[UserProfile] ON [dbo].[CompanyToProfiles].UserProfileID = [UserProfile].[UserProfile].UserProfileID
WHERE  [dbo].[BuildingToCompanies].BuildingID = @BuildingId AND [UserProfile].[UserProfile].UserProfileTypeID = 4 AND [dbo].[Company].CompanyName LIKE @Search 
)
SELECT *
	FROM TenantPage
	WHERE RowNumber > @Start AND RowNumber <= @End
	ORDER BY CompanyID
RETURN 0
