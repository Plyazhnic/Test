CREATE PROCEDURE [dbo].[usp_GetAlertForUserByEntity]
    @UserProfileID int,
    @EntityTypeID int,
    @EntityID int

AS
    SELECT Alert.*, AlertType.AlertType, AlertType.AlertPeriodValue, AlertType.AlertTypeDescription FROM [dbo].[Alert]
    INNER JOIN dbo.AlertType ON dbo.Alert.AlertTypeID = dbo.AlertType.AlertTypeID
    WHERE UserProfileID=@UserProfileID AND dbo.Alert.EntityID = @EntityID AND dbo.Alert.EntityTypeID = @EntityTypeID

RETURN 0