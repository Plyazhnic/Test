CREATE PROCEDURE [dbo].[usp_GetAlertForUser]
	@UserProfileID int 
	
AS
	SELECT * FROM [dbo].[Alert] 
	INNER JOIN dbo.AlertType ON dbo.Alert.AlertTypeID = dbo.AlertType.AlertTypeID
	WHERE  Alert.isActive = '1' AND UserProfileID=@UserProfileID 
	
RETURN 0