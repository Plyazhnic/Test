CREATE PROCEDURE [dbo].[usp_AddBuildingToLot]
	@BuildingID int,
	@LotID int

AS
	INSERT INTO [dbo].[BuildingToLots]
           ([BuildingID]
           ,[LotID]
		   ,[IsActive]
		   ,[CreatedDate])
     VALUES
           (@BuildingID
           ,@LotID
		   ,1
		   ,GETDATE())
RETURN 0
