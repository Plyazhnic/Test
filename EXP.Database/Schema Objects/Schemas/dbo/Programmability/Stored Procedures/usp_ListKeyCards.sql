CREATE PROCEDURE [dbo].[usp_ListKeyCards]
	@Start int,
	@End int,
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
	SELECT KeyCardPage.*, [dbo].[Building].BuildingName, [UserProfile].[Address].ZipCode, [UserProfile].[Address].Address1, [UserProfile].[Address].Address2, [UserProfile].[Address].City, [dbo].[zState].StateName
	FROM KeyCardPage
	LEFT JOIN dbo.BuildingToLots ON KeyCardPage.LotID = dbo.BuildingToLots.LotID
	LEFT JOIN dbo.Building ON dbo.BuildingToLots.BuildingID  = dbo.Building.BuildingID
	LEFT JOIN [UserProfile].[Address] ON dbo.Building.AddressID = [UserProfile].[Address].AddressID
	LEFT JOIN dbo.zState ON [UserProfile].[Address].StateID = dbo.zState.StateID
	WHERE RowNumber > @Start AND RowNumber <= @End
	ORDER BY KeyCardPage.KeyCardID
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
	SELECT KeyCardPage.KeyCardID, KeyCardPage.KeyCardName, KeyCardPage.Rate, [dbo].[Building].BuildingName, [UserProfile].[Address].ZipCode, [UserProfile].[Address].Address1, [UserProfile].[Address].Address2, [UserProfile].[Address].City, [dbo].[zState].StateName
	FROM KeyCardPage
	LEFT JOIN dbo.Building ON KeyCardPage.BuildingID  = dbo.Building.BuildingID
	LEFT JOIN [UserProfile].[Address] ON dbo.Building.AddressID = [UserProfile].[Address].AddressID
	LEFT JOIN dbo.zState ON [UserProfile].[Address].StateID = dbo.zState.StateID
	WHERE RowNumber > @Start AND RowNumber <= @End 
	ORDER BY KeyCardPage.KeyCardID
	END

RETURN 0
