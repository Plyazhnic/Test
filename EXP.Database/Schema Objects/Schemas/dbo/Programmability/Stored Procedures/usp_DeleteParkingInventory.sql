CREATE PROCEDURE [dbo].[usp_DeleteParkingInventory]
	@ParkingInventoryID int 
AS
	UPDATE [dbo].[ParkingInventory]
	SET isActive = 0
      WHERE ParkingInventoryID = @ParkingInventoryID
RETURN 0
