CREATE PROCEDURE [dbo].[usp_UpdateBuilding]
	@BuildingID int,
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
	UPDATE [dbo].[Building]
   SET [BuildingName] = @BuildingName
      ,[OwnerID] = @OwnerID
      ,[ManagerID] = @ManagerID
      ,[AddressID] = @AddressID
      ,[MailingAddressID] = @MailingAddressID
      ,[PrimaryPhoneNumber] = @PrimaryPhoneNumber
      ,[PrimaryFaxNumber] = @PrimaryFaxNumber
      ,[MailingPhoneNumber] = @MailingPhoneNumber
      ,[MailingFaxNumber] = @MailingFaxNumber
	  ,[BankName] = @BankName
      ,[AccountName] = @AccountName
      ,[AccountNumber] = @AccountNumber
      ,[RoutingNumber] = @RoutingNumber
      ,[Notes] = @Notes
      ,[TaxRate] = @TaxRate
      ,[DueDateReminder] = @DueDateReminder
      ,[ReminderFrequency] = @ReminderFrequency
      ,[ReminderCutoff] = @ReminderCutoff
      ,[InvoiceCutoff] = @InvoiceCutoff
      ,[ACHDiscountRate] = @ACHDiscountRate
      ,[CCDiscountRate] = @CCDiscountRate
      ,[MontlyFee] = @MontlyFee
      ,[MFBillingDay] = @MFBillingDay
      ,[VBNotification] = @VBNotification
      ,[IsActive] = @IsActive
      ,[UpdatedDate] = GETDATE()
 WHERE BuildingID = @BuildingID
RETURN 0
