CREATE PROCEDURE [dbo].[usp_UpdateParkingStall]
	@ParkingStallID int,
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
	@Description varchar(256)
AS
	UPDATE [dbo].[ParkingStall]
   SET [ParkingStallTypeID] = @ParkingStallTypeID
      ,[LotID] = @LotID
      ,[StallLocation] = @StallLocation
      ,[MontlyRate] = @MontlyRate
	  ,[Rate] = @Rate
	  ,[MaxRate] = @MaxRate
	  ,[FlatRate] = @FlatRate
	  ,[TimeIncrement] = @TimeIncrement
	  ,[GracePeriod] = @GracePeriod
      ,[StallNumber] = @StallNumber
      ,[OverSell] = @OverSell
      ,[StallDescription] = @Description
      ,[UpdatedDate] = GETDATE()
 WHERE ParkingStallID = @ParkingStallID
GO
