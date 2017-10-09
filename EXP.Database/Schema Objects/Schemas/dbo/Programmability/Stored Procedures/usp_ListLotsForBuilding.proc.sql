CREATE PROCEDURE [dbo].[usp_ListLotsForBuilding]
	@BuildingID int
AS
	SELECT dbo.Lot.LotId, dbo.Lot.LotName FROM dbo.Lot 
	LEFT JOIN dbo.BuildingToLots ON dbo.BuildingToLots.LotID = dbo.Lot.LotID
	WHERE dbo.BuildingToLots.BuildingID = @BuildingID AND dbo.Lot.IsActive = 1  ORDER BY LotName 
RETURN 0