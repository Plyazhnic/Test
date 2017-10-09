CREATE PROCEDURE [UserProfile].[usp_GetBuildingAddressByLot]
	@BuildingToLotID int = 0
AS
	SELECT [Address].* 
	FROM [UserProfile].[Address]
	INNER JOIN [dbo].[BuildingToLots] on [dbo].[BuildingToLots].BuildingToLotID = @BuildingToLotID
	INNER JOIN [dbo].[Building] on [dbo].[Building].BuildingID = [dbo].[BuildingToLots].BuildingID
	WHERE [UserProfile].[Address].AddressID = [dbo].[Building].AddressID
RETURN 0
