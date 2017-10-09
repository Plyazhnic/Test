 CREATE PROCEDURE [UserProfile].[usp_ListVehicleModelsForPage]
    @MakeId int,
	@Start int,
	@End int,
	@Search varchar(50)

AS
	SELECT @Search = '%' + RTRIM(@Search) + '%';

	WITH VehicleModelPage AS
	(
		SELECT *,
		ROW_NUMBER() OVER (ORDER BY VehicleModelID) AS RowNumber
		FROM [UserProfile].[VehicleModel] WHERE IsActive = 1 AND VehicleMakeID = @MakeId  AND VehicleModel  LIKE @Search
	)

	SELECT *
	FROM VehicleModelPage
	WHERE RowNumber > @Start AND RowNumber <= @End
	ORDER BY VehicleModelID

RETURN 0