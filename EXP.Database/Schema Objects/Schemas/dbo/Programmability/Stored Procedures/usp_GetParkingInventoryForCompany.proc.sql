CREATE PROCEDURE [dbo].[usp_GetParkingInventoryForCompany]
	@CompanyID int
AS
	DECLARE @typeId int 
	SET @typeId = (SELECT ParkingStallType.ParkingStallTypeID FROM dbo.ParkingStallType WHERE ParkingStallType = 'Visitor');

	SELECT ParkingInventory.*, Company.CompanyName,Lot.LotName,ParkingStall.StallLocation,ParkingStallType.ParkingStallType
	FROM dbo.ParkingInventory
	LEFT JOIN dbo.[Company] ON Company.CompanyID = ParkingInventory.CompanyID 
	LEFT JOIN dbo.[Lot] ON Lot.LotID = ParkingInventory.LotID
	LEFT JOIN dbo.[ParkingStall] ON ParkingStall.ParkingStallID = ParkingInventory.ParkingStallID
	LEFT JOIN dbo.[ParkingStallType] ON ParkingStallType.ParkingStallTypeID = ParkingStall.ParkingStallTypeID
	
	WHERE ParkingInventory.CompanyID = @CompanyID AND ParkingStall.ParkingStallTypeID <> @typeId AND ParkingInventory.isActive = 1

RETURN 0
