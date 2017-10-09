CREATE PROCEDURE [dbo].[usp_GetBuilding]
	@buildingID int
AS
	SELECT * FROM [dbo].[Building] WHERE BuildingID = @buildingID AND IsActive = 1

RETURN 0
