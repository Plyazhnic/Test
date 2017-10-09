CREATE PROCEDURE [UserProfile].[usp_GetVehicleModelsCount]
	@Search varchar(50),
	@MakeId int
AS
	SELECT @Search = '%' + RTRIM(@Search) + '%';
	SELECT count(*)
	FROM [UserProfile].[VehicleModel] WHERE IsActive = 1 AND VehicleMakeID = @MakeId AND VehicleModel LIKE @Search
RETURN 0
