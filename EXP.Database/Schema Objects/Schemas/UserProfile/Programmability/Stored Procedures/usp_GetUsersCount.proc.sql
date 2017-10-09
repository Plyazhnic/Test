CREATE PROCEDURE [UserProfile].[usp_GetUsersCount]
	@Filter nvarchar(max)
AS
	SELECT count(*)
	FROM [UserProfile].[UserProfile]
	WHERE @Filter is null OR FirstName like '%' + @Filter + '%' OR LastName like '%' + @Filter + '%' OR LoginName like '%' + @Filter + '%'

RETURN 0