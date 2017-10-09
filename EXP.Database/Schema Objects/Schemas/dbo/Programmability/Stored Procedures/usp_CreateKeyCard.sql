CREATE PROCEDURE [dbo].[usp_CreateKeyCard]
	@IsActive bit,
	@KeyCardName varchar(50),
	@Rate decimal(6,2),
	@LotID int
AS
  If  Exists(select * from [dbo].[KeyCards] where [dbo].[KeyCards].LotID  = @LotID)
  Begin
    insert into [dbo].[KeyCards](LotID, Rate, KeyCardName , IsActive, UpdatedDate, CreatedDate) values (@LotID, @Rate , @KeyCardName, @IsActive, NULL, GETDATE())
  End
RETURN 0
