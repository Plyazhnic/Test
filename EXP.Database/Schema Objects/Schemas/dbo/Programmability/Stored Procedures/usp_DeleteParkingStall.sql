CREATE PROCEDURE [dbo].[usp_DeleteParkingStall]
	@StallID int 
AS
	UPDATE [dbo].[ParkingStall]
	SET isActive = 0
      WHERE ParkingStallID = @StallID
RETURN 0
