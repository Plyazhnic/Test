CREATE PROCEDURE [dbo].[usp_GetBuildingAddressByLotID]
	@LotID int
AS
	WITH LotPage AS
	(
		SELECT  BuildingID
		FROM dbo.[BuildingToLots]
		WHERE  dbo.[BuildingToLots].LotID = @LotID 
	)
	SELECT [dbo].[Building].BuildingName, [UserProfile].[Address].ZipCode, [UserProfile].[Address].Address1, [UserProfile].[Address].Address2, [UserProfile].[Address].City, [dbo].[zState].StateName
	FROM LotPage
	LEFT JOIN dbo.Building ON (SELECT TOP 1 BuildingID FROM dbo.[BuildingToLots] WHERE dbo.[BuildingToLots].LotID = @LotID) = dbo.Building.BuildingID
	LEFT JOIN [UserProfile].[Address] ON dbo.Building.AddressID = [UserProfile].[Address].AddressID
	LEFT JOIN dbo.zState ON [UserProfile].[Address].StateID = dbo.zState.StateID
RETURN 0
