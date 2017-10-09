CREATE PROCEDURE [dbo].[usp_ListBuildings]
	@OwnerID int
AS
	SELECT * FROM [dbo].Building
	WHERE @OwnerID IS NULL OR OwnerID = @OwnerID
RETURN 0
