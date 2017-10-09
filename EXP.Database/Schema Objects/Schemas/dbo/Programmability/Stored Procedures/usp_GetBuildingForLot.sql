CREATE PROCEDURE [dbo].[usp_GetBuildingForLot]
	@LotID int
AS
	SELECT * FROM [dbo].[Building] 
	INNER JOIN [dbo].[BuildingToLots] on [dbo].[BuildingToLots].LotID = @LotID
	WHERE [dbo].[Building].BuildingID = [dbo].[BuildingToLots].BuildingID

RETURN 0
