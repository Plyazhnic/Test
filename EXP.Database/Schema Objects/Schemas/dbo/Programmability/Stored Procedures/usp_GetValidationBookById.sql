CREATE PROCEDURE [dbo].[usp_GetValidationBookById]
	@BookID int
AS
	SELECT * FROM [dbo].[ValidationBooks] WHERE [dbo].[ValidationBooks].BookID = @BookID
RETURN 0
