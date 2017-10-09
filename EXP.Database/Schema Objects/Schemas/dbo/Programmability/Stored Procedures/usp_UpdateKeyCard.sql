CREATE PROCEDURE [dbo].[usp_UpdateKeyCard]
	@IsActive bit,
	@KeyCardName varchar(50),
	@Rate decimal(6,2),
	@KeyCardID int
AS
	UPDATE [dbo].[KeyCards] SET Rate = @Rate, KeyCardName= @KeyCardName, IsActive = @IsActive, UpdatedDate = GETDATE()
	WHERE KeyCardID  = @KeyCardID
RETURN 0
