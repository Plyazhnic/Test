CREATE PROCEDURE [dbo].[usp_DeleteBuildingToLotEntry]
	@BuildingID int,
	@LotID int

AS
	DELETE FROM [dbo].[BuildingToLots]
      WHERE BuildingID = @BuildingID AND LotID = @LotID

