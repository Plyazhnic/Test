CREATE PROCEDURE [UserProfile].[usp_RemoveVehicle]
    @VehicleID int
AS
    DELETE FROM UserProfile.Vehicle WHERE VehicleID = @VehicleID

RETURN 0