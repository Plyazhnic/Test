CREATE PROCEDURE [dbo].[usp_GetAllocatedStall]
	@ParkingStallID int

AS
	DECLARE @count int
	SET @count = (SELECT SUM(ParkingInventory.NumberOfStalls) FROM dbo.ParkingInventory WHERE ParkingInventory.ParkingStallID = @ParkingStallID)

	SELECT TOP(1) ParkingInventory.*, Company.CompanyName,Lot.LotName,Building.BuildingName,ParkingStall.StallLocation,ParkingStallType.ParkingStallType,@count AS CountStalls
	FROM dbo.ParkingInventory
	LEFT JOIN dbo.[Company] ON Company.CompanyID = ParkingInventory.CompanyID 
	LEFT JOIN dbo.[Lot] ON Lot.LotID = ParkingInventory.LotID
	LEFT JOIN dbo.[Building] ON Building.BuildingID = ParkingInventory.BuildingID
	LEFT JOIN dbo.[ParkingStall] ON ParkingStall.ParkingStallID = ParkingInventory.ParkingStallID
	LEFT JOIN dbo.[ParkingStallType] ON ParkingStallType.ParkingStallTypeID = ParkingStall.ParkingStallTypeID
	
	WHERE ParkingInventory.ParkingStallID = @ParkingStallID

RETURN 0
