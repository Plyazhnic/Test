CREATE PROCEDURE [UserProfile].[usp_ListVehicleMakesForPage]
	@Start int,
	@End int,
	@Search varchar(50)
AS
	SELECT @Search = '%' + RTRIM(@Search) + '%';

	WITH VehicleMakePage AS
	(
		SELECT *,
		ROW_NUMBER() OVER (ORDER BY VehicleMakeID) AS RowNumber
		FROM [UserProfile].[VehicleMake] WHERE IsActive = 1 AND VehicleMake LIKE @Search
	)

	SELECT *
	FROM VehicleMakePage
	WHERE RowNumber > @Start AND RowNumber <= @End
	ORDER BY VehicleMakeID

RETURN 0
