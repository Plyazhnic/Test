CREATE PROCEDURE [dbo].[usp_UpdateCompanyManager]
	@CompanyID int,
	@ManagerID int
	
AS
	UPDATE [dbo].[Company]
   SET [ManagerID] = @ManagerID
      ,[UpdatedDateTime] = GETDATE()
 WHERE CompanyID = @CompanyID
RETURN 0
