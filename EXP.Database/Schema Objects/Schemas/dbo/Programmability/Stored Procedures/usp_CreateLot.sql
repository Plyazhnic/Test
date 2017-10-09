CREATE PROCEDURE [dbo].[usp_CreateLot]
	@LotName varchar(10),
	@ParkingOperatorID int,
	@ParkingManagerID int,
	@LotNumber varchar(10),
	@LotLevel varchar(10),
	@AddressID int,
	@IsBuilding bit,
	@MailingAddressID int,
	@ReservedPrice int,
	@UnReservedPrice int,
	@PhoneNumber varchar(20),
	@FaxNumber varchar(20),
	@MailingPhoneNumber varchar(20),
	@MailingFaxNumber varchar(20),
	@IsActive bit,
	@hasMultipleBuildings varchar(10),
	@Comments varchar(500),
	@EffectiveFrom datetime,
	@EffectiveTo datetime,
	@NewLotID int out



AS
	INSERT INTO [dbo].[Lot]
           ([LotName]
           ,[ParkingOperatorID]
           ,[ParkingManagerID]
           ,[LotNumber]
           ,[LotLevel]
           ,[AddressID]
		   ,[IsBuilding]
           ,[MailingAddressID]
           ,[ReservedPrice]
           ,[UnReservedPrice]
           ,[PhoneNumber]
           ,[FaxNumber]
           ,[MailingPhoneNumber]
           ,[MailingFaxNumber]
           ,[IsActive]
		   ,[Comments]
           ,[hasMultipleBuildings]
           ,[EffectiveFrom]
           ,[EffectiveTo])
     VALUES
           (@LotName
           ,@ParkingOperatorID
           ,@ParkingManagerID
           ,@LotNumber
           ,@LotLevel
           ,@AddressID
		   ,@IsBuilding
           ,@MailingAddressID
           ,@ReservedPrice
           ,@UnReservedPrice
           ,@PhoneNumber
           ,@FaxNumber
           ,@MailingPhoneNumber
           ,@MailingFaxNumber
           ,@IsActive
		   ,@Comments
           ,@hasMultipleBuildings
           ,@EffectiveFrom
           ,@EffectiveTo);

		   SET @NewLotID = Scope_Identity();
RETURN 0
