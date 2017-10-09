CREATE PROCEDURE [dbo].[usp_DeleteLot]
	@LotID int 
AS
	UPDATE [dbo].[Lot]
	SET isActive = 0
      WHERE LotID = @LotID
RETURN 0
