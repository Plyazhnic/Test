CREATE PROCEDURE [dbo].[usp_AddBuildingToCompany]
	@BuildingID int,
	@CompanyID int

AS
	INSERT INTO [dbo].[BuildingToCompanies]
           ([BuildingID]
           ,[CompanyID]
		   ,IsActive
		   ,[CreatedDate])
     VALUES
           (@BuildingID
           ,@CompanyID
		   ,1
		   ,GETDATE())
RETURN 0
