CREATE PROCEDURE [UserProfile].[usp_GetVehicle]
    @VehicleID int
AS
    SELECT UserProfile.Vehicle.*, UserProfile.VehicleMake.*, UserProfile.VehicleModel.*
	FROM UserProfile.Vehicle
    LEFT JOIN UserProfile.VehicleMake ON UserProfile.Vehicle.VehicleMakeID = UserProfile.VehicleMake.VehicleMakeID
    LEFT JOIN UserProfile.VehicleModel ON UserProfile.Vehicle.VehicleModelID = UserProfile.VehicleModel.VehicleModelID
    where VehicleID = @VehicleID

RETURN 0