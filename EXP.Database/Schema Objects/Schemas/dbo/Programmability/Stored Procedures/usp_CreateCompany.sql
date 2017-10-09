CREATE PROCEDURE [dbo].[usp_CreateCompany]
	@CompanyName varchar(1024),
	@AddressID int,
	@asBuilding bit,
	@WorkPhoneID int,
	@FaxID int,
	@Suite varchar(16),
	@EmailAddress varchar(128),
	@NewCompanyID int out

AS
	INSERT INTO .[dbo].[Company]
           ([CompanyName]
           ,[AddressID]
           ,[asBuilding]
		   ,[WorkPhoneID]
		   ,[FaxID]
           ,[Suite]
           ,[EmailAddress]
           ,[CreatedDate]
           ,[UpdatedDateTime])
     VALUES
           (@CompanyName
           ,@AddressID
           ,@asBuilding
		   ,@WorkPhoneID
		   ,@FaxID
           ,@Suite
           ,@EmailAddress
           ,GETDATE()
           ,GETDATE())

		   SET @NewCompanyID = Scope_Identity();

RETURN 1
