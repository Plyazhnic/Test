CREATE PROCEDURE [dbo].[usp_GetBuildingsCount]
@Search varchar(50)
AS

	SELECT @Search = '%' + RTRIM(@Search) + '%';
	SELECT count(*)
	FROM [dbo].[Building] WHERE IsActive = 1 AND BuildingName LIKE @Search

RETURN 0