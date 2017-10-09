CREATE PROCEDURE [dbo].[usp_ListCompanies]
AS
	SELECT CompanyId, CompanyName FROM dbo.Company
RETURN 0