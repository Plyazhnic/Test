CREATE PROCEDURE [dbo].[usp_GetParkingStall]
	@ParkingStallID int 
AS
	SELECT * FROM dbo.ParkingStall WHERE ParkingStallID = @ParkingStallID

RETURN 0
