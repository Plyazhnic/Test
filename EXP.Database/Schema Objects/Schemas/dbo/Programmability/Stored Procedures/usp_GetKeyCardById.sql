CREATE PROCEDURE [dbo].[usp_GetKeyCardById]
	@Cardid int
AS
	SELECT * FROM [dbo].KeyCards WHERE [dbo].KeyCards.KeyCardID = @Cardid
RETURN 0
