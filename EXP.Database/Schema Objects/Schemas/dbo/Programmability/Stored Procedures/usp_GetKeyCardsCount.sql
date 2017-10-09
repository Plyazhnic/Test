CREATE PROCEDURE [dbo].[usp_GetKeyCardsCount]
 	@LotId int,
	@Search varchar(50)
AS
	SELECT @Search = '%' + RTRIM(@Search) + '%';
	
	IF (@lotId Is NULL)
	BEGIN
	WITH KeyCardPage AS
	(
		SELECT *,
		ROW_NUMBER() OVER (ORDER BY KeyCardID) AS RowNumber
		FROM [dbo].[KeyCards] WHERE IsActive = 1 AND KeyCardName  LIKE @Search
    )
	SELECT count(*)
	FROM KeyCardPage
	LEFT JOIN dbo.BuildingToLots ON KeyCardPage.LotID = dbo.BuildingToLOts.LotID
	LEFT JOIN dbo.Building ON dbo.BuildingToLots.BuildingID  = dbo.Building.BuildingID
	LEFT JOIN [UserProfile].[Address] ON dbo.Building.AddressID = [UserProfile].[Address].AddressID
	LEFT JOIN dbo.zState ON [UserProfile].[Address].StateID = dbo.zState.StateID
	
	END
	ELSE
	
	BEGIN
	WITH KeyCardPage AS
	(
		SELECT  [dbo].[KeyCards].KeyCardName, [dbo].[KeyCards].KeyCardID, [dbo].[KeyCards].Rate, dbo.[BuildingToLots].BuildingID, 
		ROW_NUMBER() OVER (ORDER BY [dbo].KeyCards.KeyCardID) AS RowNumber
		FROM [dbo].[KeyCards]
		LEFT JOIN dbo.[BuildingToLots] ON  [dbo].[KeyCards].LotID = dbo.BuildingToLots.LotID
		WHERE [dbo].[KeyCards].IsActive = 1 AND dbo.[BuildingToLots].LotID= @LotId AND KeyCardName LIKE @Search
	)
	SELECT count(*)
	FROM KeyCardPage
	LEFT JOIN dbo.Building ON KeyCardPage.BuildingID  = dbo.Building.BuildingID
	LEFT JOIN [UserProfile].[Address] ON dbo.Building.AddressID = [UserProfile].[Address].AddressID
	LEFT JOIN dbo.zState ON [UserProfile].[Address].StateID = dbo.zState.StateID
	
	END

RETURN 0
