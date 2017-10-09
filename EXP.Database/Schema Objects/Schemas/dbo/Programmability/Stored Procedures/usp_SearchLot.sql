CREATE PROCEDURE [dbo].[usp_SearchLot]
	@LotName nvarchar(max),
    @City nvarchar(max),
    @ZipCode nvarchar(max),
    @StateID int
AS

WITH LotPage AS
	(
		SELECT [dbo].[Lot].*, dbo.[BuildingToLots].BuildingID
		
		FROM [dbo].[Lot]
		INNER JOIN dbo.[BuildingToLots] ON [BuildingToLots].LotID = dbo.Lot.LotID

		WHERE dbo.Lot.IsActive = 1 
	)
	SELECT LotPage.*, dbo.[Building].*, [UserProfile].[Address].*
	FROM LotPage
	LEFT JOIN dbo.[Building] ON LotPage.BuildingID = dbo.[Building].BuildingID
	LEFT JOIN UserProfile.[Address] ON LotPage.AddressID = UserProfile.[Address].AddressID
	
    WHERE
	    (@LotName is null or LotPage.LotName like '%' + @LotName + '%')
	AND (@City is null or [UserProfile].[Address].City like '%' + @City + '%')
	AND (@ZipCode is null or [UserProfile].[Address].ZipCode like '%' + @ZipCode + '%')
	AND (@StateID is null or [UserProfile].[Address].StateID = @StateID)
	
	ORDER BY LotID

RETURN 0