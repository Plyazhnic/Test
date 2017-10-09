CREATE PROCEDURE [UserProfile].[usp_GetVehicleMakeById]
	@MakeId int
AS
	SELECT * FROM [UserProfile].[VehicleMake] WHERE VehicleMakeID = @MakeId
RETURN 0
