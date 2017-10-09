CREATE PROCEDURE [dbo].[usp_UpdateLot]
	@LotID int,
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
	@Comments varchar(500),
	@hasMultipleBuildings varchar(10),
	@EffectiveFrom datetime,
	@EffectiveTo datetime
AS
	UPDATE [dbo].[Lot]
   SET [LotName] = @LotName
      ,[ParkingOperatorID] = @ParkingOperatorID
      ,[ParkingManagerID] = @ParkingManagerID
      ,[LotNumber] = @LotNumber
      ,[LotLevel] = @LotLevel
      ,[AddressID] = @AddressID
	  ,[IsBuilding] = @IsBuilding
      ,[MailingAddressID] = @MailingAddressID
      ,[ReservedPrice] = @ReservedPrice
      ,[UnReservedPrice] = @UnReservedPrice
      ,[PhoneNumber] = @PhoneNumber
      ,[FaxNumber] = @FaxNumber
      ,[MailingPhoneNumber] = @MailingPhoneNumber
      ,[MailingFaxNumber] = @MailingFaxNumber
      ,[IsActive] = @IsActive
	  ,[Comments] = @Comments
      ,[hasMultipleBuildings] = @hasMultipleBuildings
      ,[EffectiveFrom] = @EffectiveFrom
      ,[EffectiveTo] = @EffectiveTo
 WHERE LotID = @LotID

RETURN 0
