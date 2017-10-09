CREATE PROCEDURE [dbo].[usp_SetEmailSent]
	@EmailID int
AS
	UPDATE [dbo].[Email]
	SET [Sent] = 1, [SentDate] = GETDATE()
	WHERE [EmailID] = @EmailID

RETURN 0
