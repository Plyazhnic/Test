CREATE PROCEDURE [dbo].[usp_DeactivateAlert]
	@AlertID int
AS
	UPDATE [dbo].[Alert] 
	SET isActive = 0
    WHERE AlertID = @AlertID
	
RETURN 0