CREATE PROCEDURE [dbo].[usp_DeleteBuilding]
	@BuildingID int

AS
	UPDATE [dbo].[Building]
	SET isActive = 0
      WHERE BuildingID = @BuildingID

RETURN 0
