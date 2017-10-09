CREATE PROCEDURE [dbo].[usp_UpdateParkingInventory]
	@ParkingInventoryID int,
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
	UPDATE [dbo].[ParkingInventory]
	SET [BuildingID] = @BuildingID
		,[CompanyID] = @CompanyID
		,[LotID] = @LotID
		,[ParkingStallID] = @ParkingStallID
		,[NumberOfStalls] = @NumberOfStalls
		,[MontlyRate] = @MontlyRate
		,[MarketRate] = @MarketRate
		,[DiscountRate] = @DiscountRate
		,[OverSell] = @OverSell
		,[UserProfileID] = @UserProfileID
		,[UserVehicleID] = @UserVehicleID
		,[PermitNumber] = @PermitNumber
		,[isActive] = @isActive
		,[EffectiveFrom] = @EffectiveFrom
		,[EffectiveTo] = @EffectiveTo
		,[UpdatedDate] = GETDATE()
	WHERE ParkingInventoryID = @ParkingInventoryID
		
RETURN 0
