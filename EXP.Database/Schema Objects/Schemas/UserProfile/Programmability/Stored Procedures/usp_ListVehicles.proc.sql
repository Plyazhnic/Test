CREATE PROCEDURE [UserProfile].[usp_ListVehicles]
    @UserProfileID int
AS
    SELECT * FROM UserProfile.Vehicle
    LEFT JOIN UserProfile.VehicleMake ON UserProfile.Vehicle.VehicleMakeID = UserProfile.VehicleMake.VehicleMakeID
    LEFT JOIN UserProfile.VehicleModel ON UserProfile.Vehicle.VehicleModelID = UserProfile.VehicleModel.VehicleModelID
    where UserProfileID = @UserProfileID

RETURN 0