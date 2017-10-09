CREATE PROCEDURE [dbo].[usp_DeleteKeyCard]
	@CardId int 
AS
	UPDATE [dbo].KeyCards
	SET    IsActive = 0
    WHERE  KeyCardID = @CardId
RETURN 0
