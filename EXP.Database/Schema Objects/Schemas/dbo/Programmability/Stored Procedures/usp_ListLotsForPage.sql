CREATE PROCEDURE [dbo].[usp_ListLotsForPage]
	@Start int,
	@End int,
	@Search varchar(50),
	@BuildingId Int  
AS	
	SELECT @Search = '%' + RTRIM(@Search) + '%';
	
	IF (@BuildingId Is NULL)
	BEGIN
	WITH LotPage AS
	(
		SELECT *,
		ROW_NUMBER() OVER (ORDER BY LotID) AS RowNumber
		FROM [dbo].[Lot] WHERE IsActive = 1 AND [dbo].[Lot].LotName LIKE @Search
    )
	SELECT LotPage.*, dbo.Building.*, [UserProfile].[Address].*
	FROM LotPage
	LEFT JOIN dbo.[Building] ON (SELECT TOP 1 BuildingID FROM dbo.[BuildingToLots] WHERE dbo.[BuildingToLots].LotID = LotPage.LotID) = dbo.[Building].BuildingID 
	LEFT JOIN UserProfile.[Address] ON LotPage.AddressID = UserProfile.[Address].AddressID
	WHERE RowNumber > @Start AND RowNumber <= @End
	ORDER BY LotID
	END
	ELSE
	
	BEGIN
	WITH LotPage AS
	(
		SELECT [dbo].[Lot].*, dbo.[BuildingToLots].BuildingID,
		ROW_NUMBER() OVER (ORDER BY dbo.Lot.LotID) AS RowNumber
		FROM [dbo].[Lot]
		INNER JOIN dbo.[BuildingToLots] ON [BuildingToLots].LotID = dbo.Lot.LotID
		WHERE dbo.Lot.IsActive = 1 AND dbo.[BuildingToLots].BuildingID = @BuildingId AND [dbo].[Lot].LotName LIKE @Search
	)
	SELECT LotPage.*, dbo.Building.*, [UserProfile].[Address].*
	FROM LotPage
	LEFT JOIN dbo.[Building] ON LotPage.BuildingID = dbo.[Building].BuildingID
	LEFT JOIN UserProfile.[Address] ON LotPage.AddressID = UserProfile.[Address].AddressID
	WHERE RowNumber > @Start AND RowNumber <= @End 
	ORDER BY LotID
	END


RETURN 0