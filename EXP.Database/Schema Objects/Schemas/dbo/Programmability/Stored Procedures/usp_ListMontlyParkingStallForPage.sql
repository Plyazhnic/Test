CREATE PROCEDURE [dbo].[usp_ListMontlyParkingStallForPage]
	@LotID int,
	@Start int,
	@End int,
	@Search varchar(50)
AS
	SELECT @Search = '%' + RTRIM(@Search) + '%';
	DECLARE @typeId int 
	SET @typeId = (SELECT ParkingStallType.ParkingStallTypeID FROM dbo.ParkingStallType WHERE ParkingStallType = 'Visitor');

	WITH MontlyStallPage AS
	(
		SELECT *,
		ROW_NUMBER() OVER (ORDER BY ParkingStallID) AS RowNumber
		FROM [dbo].[ParkingStall] WHERE StallLocation LIKE @Search AND LotID = @LotID AND ParkingStall.ParkingStallTypeID <> @typeId AND isActive = 1
	)
    
	SELECT MontlyStallPage.*, dbo.ParkingStallType.ParkingStallType,(SELECT SUM(ParkingInventory.NumberOfStalls) FROM dbo.ParkingInventory WHERE ParkingInventory.ParkingStallID = MontlyStallPage.ParkingStallID) AS BookedStalls
	FROM MontlyStallPage
	LEFT JOIN dbo.[ParkingStallType] ON ParkingStallType.ParkingStallTypeID = MontlyStallPage.ParkingStallTypeID
	WHERE RowNumber > @Start AND RowNumber <= @End

RETURN 0
