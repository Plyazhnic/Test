CREATE PROCEDURE [dbo].[usp_GetBuildingLotsCount]
@Search varchar(50),
@BuildingID int
AS

	SELECT @Search = '%' + RTRIM(@Search) + '%';
	SELECT count(*)
	FROM [dbo].[Lot]
	INNER JOIN BuildingToLots ON Lot.LotID = BuildingToLots.LotID
	WHERE Lot.IsActive = 1 AND LotName LIKE @Search AND BuildingToLots.BuildingID = @BuildingID

RETURN 0