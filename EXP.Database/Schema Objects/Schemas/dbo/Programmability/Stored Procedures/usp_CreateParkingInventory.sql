CREATE PROCEDURE [dbo].[usp_CreateParkingInventory]
	@BuildingID int,
	@ParkingStallID int,
	@NumberOfStalls int,
	@MontlyRate decimal(5,2),
	@MarketRate bit,
	@DiscountRate decimal(5,2),
	@OverSell decimal(5,2),
	@UserVehicleID int,
	@UserProfileID int,
	@PermitNumber varchar(32),
	@LotID int,
	@CompanyID int,
	@isActive bit,
	@EffectiveFrom datetime,
	@EffectiveTo datetime
AS
	INSERT INTO [dbo].[ParkingInventory]
		   ([BuildingID]
           ,[CompanyID]
           ,[LotID]
           ,[ParkingStallID]
           ,[NumberOfStalls]
           ,[MontlyRate]
           ,[MarketRate]
           ,[DiscountRate]
           ,[OverSell]
           ,[UserProfileID]
           ,[UserVehicleID]
           ,[PermitNumber]
           ,[isActive]
           ,[EffectiveFrom]
           ,[EffectiveTo]
		   ,[CreatedDate])
     VALUES
           (@BuildingID
           ,@CompanyID
           ,@LotID
           ,@ParkingStallID
           ,@NumberOfStalls
           ,@MontlyRate
           ,@MarketRate
           ,@DiscountRate
           ,@OverSell
           ,@UserProfileID
           ,@UserVehicleID
           ,@PermitNumber
           ,@isActive
           ,@EffectiveFrom
           ,@EffectiveTo
		   ,GETDATE())
RETURN 0
