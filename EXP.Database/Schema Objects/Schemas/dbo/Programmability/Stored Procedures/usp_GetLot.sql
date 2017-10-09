CREATE PROCEDURE [dbo].[usp_GetLot]
	@LotID int

AS
	SELECT dbo.[Lot].*, [UserProfile].[Address].*, dbo.[zState].*
	FROM dbo.[Lot]
	LEFT JOIN UserProfile.[Address] ON UserProfile.[Address].AddressID = dbo.[Lot].AddressID 
	LEFT JOIN dbo.[zState] ON dbo.[zState].StateID = [Address].StateID
	WHERE LotID = @LotID
RETURN 0
