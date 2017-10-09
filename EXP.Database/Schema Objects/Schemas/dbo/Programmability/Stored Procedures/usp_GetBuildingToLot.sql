CREATE PROCEDURE [dbo].[usp_GetBuildingToLot]
	@BuildingToLotID int

AS
	SELECT dbo.BuildingToLots.*, dbo.Lot.*, dbo.Building.*
	FROM dbo.BuildingToLots 
	LEFT JOIN dbo.Lot ON dbo.BuildingToLots.LotID = dbo.Lot.LotID
	LEFT JOIN dbo.Building ON dbo.BuildingToLots.BuildingID = dbo.Building.BuildingID
	WHERE dbo.BuildingToLots.IsActive = 1 AND dbo.BuildingToLots.BuildingToLotID = @BuildingToLotID
RETURN 0
