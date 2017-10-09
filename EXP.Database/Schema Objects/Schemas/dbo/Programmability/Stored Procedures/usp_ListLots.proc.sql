CREATE PROCEDURE [dbo].[usp_ListLots]
AS
	SELECT LotId, LotName FROM dbo.Lot WHERE IsActive = 1
RETURN 0