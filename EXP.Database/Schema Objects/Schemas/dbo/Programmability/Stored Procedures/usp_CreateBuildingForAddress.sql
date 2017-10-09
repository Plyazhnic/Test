CREATE PROCEDURE [dbo].[usp_CreateBuildingForAddress]
	@BuildingName varchar(256),
	@AddressID int,
	@IsActive bit,
	@OwnerID int
AS
	INSERT INTO [dbo].[Building]
           ([BuildingName]

           ,[AddressID],
		    [IsActive],
			[CreatedDate],
			[UpdatedDate],
			[OwnerID])
    VALUES
           (@BuildingName
           ,@AddressID
           ,@IsActive
     	   ,GETDATE(),
            NULL,
		    @OwnerID)

	SELECT CAST(SCOPE_IDENTITY() as int)
RETURN 0
