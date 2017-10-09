CREATE PROCEDURE [UserProfile].[usp_ListUsers]
	@Start int,
	@End int,
	@Filter nvarchar(max)
AS
	WITH UserPage AS
	(
		SELECT *,
		ROW_NUMBER() OVER (ORDER BY UserProfileID) AS RowNumber
		FROM [UserProfile].[UserProfile]
		WHERE @Filter is null OR FirstName like '%' + @Filter + '%' OR LastName like '%' + @Filter + '%' OR LoginName like '%' + @Filter + '%'
	)

	SELECT *
	FROM UserPage
	WHERE RowNumber > @Start AND RowNumber <= @End
	ORDER BY UserProfileID

RETURN 0
