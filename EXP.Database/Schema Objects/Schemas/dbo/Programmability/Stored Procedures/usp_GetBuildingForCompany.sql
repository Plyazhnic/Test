CREATE PROCEDURE [dbo].[usp_GetBuildingForCompany]
	@CompanyID int
AS
	SELECT * FROM [dbo].[Building] 
	INNER JOIN [dbo].[BuildingToCompanies] on [dbo].[BuildingToCompanies].CompanyID = @CompanyID
	WHERE [dbo].[Building].BuildingID = [dbo].[BuildingToCompanies].BuildingID

RETURN 0
