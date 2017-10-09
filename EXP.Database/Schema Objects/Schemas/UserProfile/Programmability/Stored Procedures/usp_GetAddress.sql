CREATE PROCEDURE [UserProfile].[usp_GetAddress]
	@AddressID int
AS
	SELECT [Address].*, zState.*
	From [UserProfile].[Address] 
	LEFT JOIN dbo.[zState] ON dbo.[zState].StateID = [UserProfile].[Address].StateID
	WHERE AddressID = @AddressID
RETURN 0
