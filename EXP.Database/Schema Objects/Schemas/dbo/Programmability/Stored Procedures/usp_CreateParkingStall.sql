CREATE PROCEDURE [dbo].[usp_CreateParkingStall]
	@LotID int,
	@StallLocation varchar(256),
	@ParkingStallTypeID int,
	@StallNumber int,
	@MontlyRate decimal,
	@OverSell decimal,
	@Rate decimal,
	@MaxRate decimal,
	@FlatRate decimal,
	@TimeIncrement int,
	@GracePeriod int,
	@Description varchar(256),
	@IsActive bit,
	@NewStallID int out

AS
	INSERT INTO [dbo].[ParkingStall]
           ([LotID]
           ,[StallLocation]
		   ,[ParkingStallTypeID]
           ,[StallNumber]
           ,[MontlyRate]
           ,[OverSell]
           ,[Rate]
           ,[MaxRate]
           ,[FlatRate]
           ,[TimeIncrement]
           ,[GracePeriod]
           ,[StallDescription]
           ,[isActive]
           ,[CreatedDate])
     VALUES
           (@LotID
           ,@StallLocation
		   ,@ParkingStallTypeID
		   ,@StallNumber
           ,@MontlyRate
           ,@OverSell
		   ,@Rate
		   ,@MaxRate
		   ,@FlatRate
		   ,@TimeIncrement
		   ,@GracePeriod
           ,@Description
		   ,@IsActive
           ,GETDATE())

		   SET @NewStallID = Scope_Identity();
GO
