CREATE PROCEDURE [dbo].[usp_GetParkingInventoryByID]
	@ParkingInventoryID int
AS

	SELECT ParkingInventory.*, Company.CompanyName,Lot.LotName,ParkingStall.StallLocation,ParkingStallType
	FROM dbo.ParkingInventory
	LEFT JOIN dbo.[Company] ON Company.CompanyID = ParkingInventory.CompanyID 
	LEFT JOIN dbo.[Lot] ON Lot.LotID = ParkingInventory.LotID
	LEFT JOIN dbo.[ParkingStall] ON ParkingStall.ParkingStallID = ParkingInventory.ParkingStallID
	LEFT JOIN dbo.[ParkingStallType] ON ParkingStallType.ParkingStallTypeID = ParkingStall.ParkingStallTypeID

	WHERE ParkingInventory.ParkingInventoryID = @ParkingInventoryID AND ParkingInventory.isActive = 1

RETURN 0
