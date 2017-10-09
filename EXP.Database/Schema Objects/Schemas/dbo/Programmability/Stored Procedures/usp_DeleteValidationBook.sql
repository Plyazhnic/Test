CREATE PROCEDURE [dbo].[usp_DeleteValidationBook]
	@BookID int 
AS
	UPDATE [dbo].[ValidationBooks]
	SET    IsActive = 0
    WHERE  BookID = @BookID
RETURN 0
