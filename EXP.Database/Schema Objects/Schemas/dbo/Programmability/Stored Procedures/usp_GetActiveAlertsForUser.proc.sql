CREATE PROCEDURE [dbo].[usp_GetActiveAlertForUser]
    @UserProfileID int 

AS
    SELECT Alert.*, AlertType.AlertType, AlertType.AlertPeriodValue, AlertType.AlertTypeDescription FROM [dbo].[Alert] 
    INNER JOIN dbo.AlertType ON dbo.Alert.AlertTypeID = dbo.AlertType.AlertTypeID
    WHERE  Alert.isActive = '1' AND UserProfileID=@UserProfileID 

RETURN 0