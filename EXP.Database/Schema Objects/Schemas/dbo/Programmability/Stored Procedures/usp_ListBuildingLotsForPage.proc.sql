CREATE PROCEDURE [dbo].[usp_ListBuildingLotsForPage]
	@Start int,
	@End int,
	@Search varchar(50),
	@BuildingID int
AS	
	SELECT @Search = '%' + RTRIM(@Search) + '%';

	WITH LotPage AS
	(
		SELECT Lot.*,
		ROW_NUMBER() OVER (ORDER BY Lot.LotID) AS RowNumber
		FROM [dbo].[Lot]
		INNER JOIN BuildingToLots ON Lot.LotID = BuildingToLots.LotID
		WHERE Lot.IsActive = 1 AND LotName LIKE @Search AND BuildingToLots.BuildingID = @BuildingID
	)

	SELECT LotPage.*
	FROM LotPage
	WHERE RowNumber > @Start AND RowNumber <= @End
	ORDER BY LotID

RETURN 0