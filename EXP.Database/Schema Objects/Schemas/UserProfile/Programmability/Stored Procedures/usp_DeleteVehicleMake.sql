CREATE PROCEDURE [UserProfile].[usp_DeleteVehicleMake]
	@VehicleMakeID int 
AS
	UPDATE [UserProfile].[VehicleMake]
	SET isActive = 0
      WHERE VehicleMakeID = @VehicleMakeID
RETURN 0
