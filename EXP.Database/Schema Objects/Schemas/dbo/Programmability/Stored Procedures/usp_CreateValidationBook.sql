CREATE PROCEDURE [dbo].[usp_CreateValidationBook]
@LotID int,
@Rate decimal(6,2),
@TicketCount int,
@BookName varchar(50),
@IsActive bit
AS
  If  Exists(select * from [dbo].[ValidationBooks] where [dbo].[ValidationBooks].LotID  = @LotID)
  Begin
    insert into [dbo].[ValidationBooks] (LotID, Rate, TicketCount, BookName, IsActive, UpdatedDate, CreatedDate) values (@LotID, @Rate ,@TicketCount, @BookName, @IsActive, NULL, GETDATE())
  End
RETURN 0
