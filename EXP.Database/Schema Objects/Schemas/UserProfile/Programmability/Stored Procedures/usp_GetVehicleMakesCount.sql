CREATE PROCEDURE [UserProfile].[usp_GetVehicleMakesCount]
	@Search varchar(50)
	
	AS
	SELECT @Search = '%' + RTRIM(@Search) + '%';
	SELECT count(*)
	FROM [UserProfile].[VehicleMake] WHERE IsActive = 1 AND VehicleMake LIKE @Search
RETURN 0
