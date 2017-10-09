CREATE PROCEDURE [dbo].[usp_GetParkingInfoForLot]
	@LotID int 
AS

	DECLARE @ReservedRate decimal
	SET @ReservedRate = (SELECT MontlyRate FROM dbo.ParkingStall WHERE LotID = @LotID AND ParkingStallTypeID = 1)
	
	DECLARE @ReservedSpace int
	SET @ReservedSpace =(SELECT StallNumber FROM [dbo].[ParkingStall] WHERE LotID = @LotID AND ParkingStallTypeID = 1)- (SELECT SUM(ParkingInventory.NumberOfStalls) FROM dbo.ParkingInventory WHERE ParkingInventory.ParkingStallID = 1)

	SELECT @ReservedRate AS ReservedRate, @ReservedSpace AS ReservedSpace, MontlyRate AS UnreservedRate, StallNumber - (SELECT SUM(ParkingInventory.NumberOfStalls) FROM dbo.ParkingInventory 
		WHERE ParkingInventory.ParkingStallID = [dbo].[ParkingStall].ParkingStallID) AS UnreservedSpace
	
	FROM [dbo].[ParkingStall] 
	WHERE LotID = @LotID AND ParkingStallTypeID = 2

RETURN 0
