CREATE PROCEDURE [dbo].[usp_ListBuildingCompanies]
	@BuildingId int,
	@FromDate datetime
AS
	SELECT Company.* FROM dbo.Company
	INNER JOIN dbo.BuildingToCompanies ON Company.CompanyID = BuildingToCompanies.CompanyID
	WHERE BuildingID = @BuildingId
	AND (Company.CreatedDate >= @FromDate OR @FromDate IS NULL)
RETURN 0
