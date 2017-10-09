CREATE PROCEDURE [dbo].[usp_CreateBuilding]
	@BuildingName varchar(256),
	@OwnerID int,
	@ManagerID int,
	@AddressID int,
	@MailingAddressID int,
	@PrimaryPhoneNumber varchar(20),
	@PrimaryFaxNumber varchar(20),
	@MailingPhoneNumber varchar(20),
	@MailingFaxNumber varchar(20),
	@BankName nvarchar(50),
    @AccountName nvarchar(20),
	@AccountNumber nvarchar(20),
	@RoutingNumber nvarchar(20),
	@Notes nvarchar(256),
	@TaxRate decimal(5,2),
	@DueDateReminder int,
	@ReminderFrequency int,
	@ReminderCutoff int,
	@InvoiceCutoff int,
	@ACHDiscountRate decimal(5,2),
	@CCDiscountRate decimal(5,2),
	@MontlyFee decimal(6,2),
	@MFBillingDay int,
	@VBNotification nvarchar(256),
	@IsActive bit
AS
	INSERT INTO [dbo].[Building]
           ([BuildingName]
           ,[OwnerID]
           ,[ManagerID]
           ,[AddressID]
           ,[MailingAddressID]
           ,[PrimaryPhoneNumber]
           ,[PrimaryFaxNumber]
           ,[MailingPhoneNumber]
           ,[MailingFaxNumber]
		   ,[BankName]
		   ,[AccountName]
		   ,[AccountNumber]
           ,[RoutingNumber]
           ,[Notes]
           ,[TaxRate]
           ,[DueDateReminder]
           ,[ReminderFrequency]
           ,[ReminderCutoff]
           ,[InvoiceCutoff]
           ,[ACHDiscountRate]
           ,[CCDiscountRate]
           ,[MontlyFee]
           ,[MFBillingDay]
           ,[VBNotification]
           ,[IsActive]
           ,[HasLots]
           ,[HasMultipleTenants]
           ,[UpdatedDate]
           ,[CreatedDate])
     VALUES
           (@BuildingName
           ,@OwnerID
           ,@ManagerID
           ,@AddressID
           ,@MailingAddressID
           ,@PrimaryPhoneNumber
           ,@PrimaryFaxNumber
           ,@MailingPhoneNumber
           ,@MailingFaxNumber
		   ,@BankName
		   ,@AccountName
		   ,@AccountNumber
		   ,@RoutingNumber
		   ,@Notes
		   ,@TaxRate
		   ,@DueDateReminder
		   ,@ReminderFrequency
		   ,@ReminderCutoff
		   ,@InvoiceCutoff
		   ,@ACHDiscountRate
		   ,@CCDiscountRate
		   ,@MontlyFee
		   ,@MFBillingDay
		   ,@VBNotification
           ,@IsActive
           ,NULL
           ,NULL
		   ,NULL
           ,GETDATE())

	SELECT CAST(SCOPE_IDENTITY() as int)
RETURN 0
