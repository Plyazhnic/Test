CREATE PROCEDURE [dbo].[usp_ListBuildingsForPage]
	@Start int,
	@End int,
	@Search varchar(50)
AS
	
	SELECT @Search = '%' + RTRIM(@Search) + '%';

	DECLARE @buildingID int; 

	WITH BuildingPage AS
	(
		SELECT *,
		ROW_NUMBER() OVER (ORDER BY BuildingID) AS RowNumber
		FROM [dbo].[Building] WHERE IsActive = 1 AND BuildingName LIKE @Search
	)

	SELECT BuildingPage.*, UserProfile.[Address].*, UserProfile.UserProfile.*
	FROM BuildingPage
	LEFT JOIN UserProfile.[Address] ON BuildingPage.AddressID = UserProfile.[Address].AddressID
	LEFT JOIN UserProfile.UserProfile ON BuildingPage.OwnerID = UserProfile.UserProfile.UserProfileID
	WHERE RowNumber > @Start AND RowNumber <= @End
	ORDER BY BuildingID

RETURN 0