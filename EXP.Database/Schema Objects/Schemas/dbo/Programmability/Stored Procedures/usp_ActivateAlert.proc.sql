CREATE PROCEDURE [dbo].[usp_ActivateAlert]
	@AlertID int
AS
	UPDATE [dbo].[Alert] 
	SET isActive = 1
    WHERE AlertID = @AlertID
RETURN 0