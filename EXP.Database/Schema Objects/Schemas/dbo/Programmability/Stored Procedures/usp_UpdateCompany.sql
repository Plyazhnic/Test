CREATE PROCEDURE [dbo].[usp_UpdateCompany]
	@CompanyID int,
	@CompanyName varchar(1024),
	@AddressID int,
	@asBuilding bit,
	@Suite varchar(16),
	@EmailAddress varchar(128)
	
AS
	UPDATE [dbo].[Company]
   SET [CompanyName] = @CompanyName
      ,[AddressID] = @AddressID
      ,[asBuilding] = @asBuilding
      ,[Suite] = @Suite
      ,[EmailAddress] = @EmailAddress
      ,[UpdatedDateTime] = GETDATE()
 WHERE CompanyID = @CompanyID
RETURN 0
