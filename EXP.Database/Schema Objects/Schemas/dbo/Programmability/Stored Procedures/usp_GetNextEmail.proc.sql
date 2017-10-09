CREATE PROCEDURE [dbo].[usp_GetNextEmail]
AS
	SELECT TOP 1 * from [dbo].[Email]
	WHERE [Sent] = 0
	ORDER by EmailID

RETURN 0
