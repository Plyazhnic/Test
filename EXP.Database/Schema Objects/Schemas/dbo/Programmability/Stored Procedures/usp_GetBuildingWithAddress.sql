CREATE PROCEDURE [dbo].[usp_GetBuildingWithAddress]
	@BuildingID int 

AS
	SELECT  [dbo].[Building].[BuildingID],  [dbo].[Building].[BuildingName], [UserProfile].[Address].Address1, [UserProfile].[Address].City, [UserProfile].[Address].StateID, [UserProfile].[Address].ZipCode
	
	FROM dbo.Building
	INNER JOIN  [UserProfile].[Address] ON [dbo].[Building].[AddressID] = [UserProfile].[Address].[AddressID]  
	WHERE [dbo].[Building].[BuildingID] = @BuildingID
RETURN 0
