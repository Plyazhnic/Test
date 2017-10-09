CREATE PROCEDURE [UserProfile].[usp_GetVehicleModelById]
	@ModelId int
AS
	SELECT * FROM [UserProfile].[VehicleModel] WHERE VehicleModelID = @ModelId
RETURN 0
