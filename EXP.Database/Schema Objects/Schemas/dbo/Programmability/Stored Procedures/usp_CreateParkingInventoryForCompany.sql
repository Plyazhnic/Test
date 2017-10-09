CREATE PROCEDURE [dbo].[usp_CreateParkingInventoryForCompany]
	@UserProfileID int,
	@LotID int,
	@CompanyID int,
	@DiscountRate decimal(5,2),
	@MarketRate bit,
	@ReservedSpace int,
	@UnreservedSpace int,
	@StartDate datetime,
	@EndDate datetime 
AS
	DECLARE @stallId int
	DECLARE @buildingId int
	DECLARE @rate decimal
	DECLARE @overSell decimal

	SELECT TOP 1 @buildingId = BuildingID FROM [dbo].[BuildingToLots]  WHERE LotID = @LotID

	SELECT @stallId = ParkingStallID, @rate = MontlyRate, @overSell = OverSell FROM [dbo].[ParkingStall]  WHERE LotID = @LotID AND ParkingStallTypeID = 1
	
	INSERT INTO [dbo].[ParkingInventory]
		([BuildingID]
		,[CompanyID]
		,[LotID]
		,[ParkingStallID]
		,[UserProfileID]
		,[NumberOfStalls]
		,[MontlyRate]
		,[MarketRate]
		,[DiscountRate]
		,[OverSell]   
		,[isActive]
		,[EffectiveFrom]
		,[EffectiveTo]
		,[CreatedDate])
	VALUES
		(@buildingId
		,@CompanyID
		,@LotID
		,@stallId
		,@UserProfileID
		,@ReservedSpace
		,@rate
		,@MarketRate
		,@DiscountRate
		,@overSell
		,1
		,@StartDate
		,@EndDate
		,GETDATE())

	SELECT @stallId = ParkingStallID, @rate = MontlyRate, @overSell = OverSell FROM [dbo].[ParkingStall]  WHERE LotID = @LotID AND ParkingStallTypeID = 2
	
	INSERT INTO [dbo].[ParkingInventory]
		([BuildingID]
		,[CompanyID]
		,[LotID]
		,[ParkingStallID]
		,[UserProfileID]
		,[NumberOfStalls]
		,[MontlyRate]
		,[MarketRate]
		,[DiscountRate]
		,[OverSell]   
		,[isActive]
		,[EffectiveFrom]
		,[EffectiveTo]
		,[CreatedDate])
	VALUES
		(@buildingId
		,@CompanyID
		,@LotID
		,@stallId
		,@UserProfileID
		,@UnreservedSpace
		,@rate
		,@MarketRate
		,@DiscountRate
		,@overSell
		,1
		,@StartDate
		,@EndDate
		,GETDATE()	)
			
RETURN 0
