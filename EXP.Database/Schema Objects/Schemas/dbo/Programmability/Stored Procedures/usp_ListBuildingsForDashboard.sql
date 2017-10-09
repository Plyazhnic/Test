CREATE PROCEDURE [dbo].[usp_ListBuildingsForDashboard]
	@OwnerID int
AS
	SELECT [dbo].Building.BuildingID, [dbo].Building.BuildingName, [UserProfile].[Address].Address1, [UserProfile].[Address].Address2, [UserProfile].[Address].City, dbo.zState.StateName, [UserProfile].[Address].ZipCode   
	FROM [dbo].Building
	
	INNER JOIN [UserProfile].[Address] ON  [dbo].Building.AddressID = [UserProfile].[Address].AddressID
	INNER JOIN dbo.zState ON [UserProfile].[Address].StateID = dbo.zState.StateID 
	
	WHERE  OwnerID = @OwnerID AND [dbo].[Building].IsActive = 1
RETURN 0
