CREATE PROCEDURE [UserProfile].[usp_ListModels]
	@makeID int
AS
	SELECT VehicleModel, VehicleModelID, VehicleMakeID, [Year1] FROM UserProfile.VehicleModel WHERE VehicleMakeID=@makeID

RETURN 0