CREATE PROCEDURE [dbo].[usp_UpdateValidationBook]
@BookID int,
@Rate decimal(6,2),
@TicketCount int,
@BookName varchar(50),
@IsActive bit
AS
	UPDATE dbo.ValidationBooks SET Rate = @Rate, TicketCount = @TicketCount, BookName = @BookName, IsActive = @IsActive, UpdatedDate = GETDATE()
	WHERE BookID  = @BookID
RETURN 0
