CREATE PROCEDURE [dbo].[usp_UpdateCompanyEmployee]
	@CompanyID int,
	@EmployeeID int
	
AS
	UPDATE [dbo].[Company]
   SET [EmployeeID] = @EmployeeID
      ,[UpdatedDateTime] = GETDATE()
 WHERE CompanyID = @CompanyID
RETURN 0
