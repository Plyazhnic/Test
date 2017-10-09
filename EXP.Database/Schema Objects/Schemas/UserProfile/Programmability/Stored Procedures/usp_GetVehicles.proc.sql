CREATE PROCEDURE [UserProfile].[usp_GetVehicles]
    @UserProfileID int
AS
    SELECT TOP(2)* FROM UserProfile.Vehicle
    LEFT JOIN UserProfile.VehicleMake ON UserProfile.Vehicle.VehicleMakeID = UserProfile.VehicleMake.VehicleMakeID
    LEFT JOIN UserProfile.VehicleModel ON UserProfile.Vehicle.VehicleModelID = UserProfile.VehicleModel.VehicleModelID
    where UserProfileID = @UserProfileID ORDER BY VehicleID

RETURN 0