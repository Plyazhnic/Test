CREATE PROCEDURE [dbo].[usp_GetCompany]
	@CompanyID int
AS
	SELECT * FROM [dbo].[Company] WHERE CompanyID = @CompanyID

RETURN 0
