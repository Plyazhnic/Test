CREATE PROCEDURE [dbo].[usp_ListValidationBooks]
	@Start int,
	@End int,
	@LotId int,
	@Search varchar(50)
AS
	SELECT @Search = '%' + RTRIM(@Search) + '%';
	
	IF (@lotId Is NULL)
	BEGIN
	WITH BookPage AS
	(
		SELECT *,
		ROW_NUMBER() OVER (ORDER BY BookID) AS RowNumber
		FROM [dbo].[ValidationBooks] WHERE IsActive = 1 AND BookName LIKE @Search
    )
	SELECT BookPage.*, [dbo].[Building].BuildingName, [UserProfile].[Address].ZipCode, [UserProfile].[Address].Address1, [UserProfile].[Address].Address2, [UserProfile].[Address].City, [dbo].[zState].StateName
	FROM BookPage
	LEFT JOIN dbo.BuildingToLots ON BookPage.LotID = dbo.BuildingToLots.LotID
	LEFT JOIN dbo.Building ON dbo.BuildingToLots.BuildingID  = dbo.Building.BuildingID
	LEFT JOIN [UserProfile].[Address] ON dbo.Building.AddressID = [UserProfile].[Address].AddressID
	LEFT JOIN dbo.zState ON [UserProfile].[Address].StateID = dbo.zState.StateID
	WHERE RowNumber > @Start AND RowNumber <= @End
	ORDER BY BookPage.BookID
	END
	ELSE
	
	BEGIN
	WITH BookPage AS
	(
		SELECT  [dbo].[ValidationBooks].BookID, [dbo].[ValidationBooks].BookName, [dbo].[ValidationBooks].TicketCount, [dbo].[ValidationBooks].Rate, dbo.[BuildingToLots].BuildingID,
		ROW_NUMBER() OVER (ORDER BY [dbo].ValidationBooks.BookID) AS RowNumber
		FROM [dbo].ValidationBooks
		LEFT JOIN dbo.[BuildingToLots] ON  dbo.ValidationBooks.LotID = dbo.BuildingToLots.LotID
		WHERE dbo.ValidationBooks.IsActive = 1 AND dbo.[BuildingToLots].LotID= @LotId AND BookName LIKE @Search
	)
	SELECT BookPage.BookID, BookPage.BookName, BookPage.TicketCount, BookPage.Rate, [dbo].[Building].BuildingName, [UserProfile].[Address].ZipCode, [UserProfile].[Address].Address1, [UserProfile].[Address].Address2, [UserProfile].[Address].City, [dbo].[zState].StateName
	FROM BookPage
	LEFT JOIN dbo.Building ON BookPage.BuildingID  = dbo.Building.BuildingID
	LEFT JOIN [UserProfile].[Address] ON dbo.Building.AddressID = [UserProfile].[Address].AddressID
	LEFT JOIN dbo.zState ON [UserProfile].[Address].StateID = dbo.zState.StateID
	WHERE RowNumber > @Start AND RowNumber <= @End 
	ORDER BY BookPage.BookID
	END

RETURN 0
