CREATE PROCEDURE [dbo].[usp_ListParkingStallByType]
	@ParkingStallTypeID int,
	@LotID int
AS
	SELECT dbo.ParkingStall.*, dbo.ParkingStallType.ParkingStallType 
	FROM dbo.ParkingStall 
	LEFT JOIN dbo.[ParkingStallType] ON ParkingStallType.ParkingStallTypeID = ParkingStall.ParkingStallTypeID
	WHERE LotID = @LotID AND ParkingStall.ParkingStallTypeID = @ParkingStallTypeID

RETURN 0
