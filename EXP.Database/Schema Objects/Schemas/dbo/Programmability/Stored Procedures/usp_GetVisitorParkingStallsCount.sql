CREATE PROCEDURE [dbo].[usp_GetVisitorParkingStallsCount]
	@Search varchar(50),
	@LotID int

	AS
	DECLARE @typeId int 
	SET @typeId = (SELECT ParkingStallType.ParkingStallTypeID FROM dbo.ParkingStallType WHERE ParkingStallType = 'Visitor')

	SELECT @Search = '%' + RTRIM(@Search) + '%';
	SELECT count(*)
	FROM [dbo].[ParkingStall] WHERE StallLocation LIKE @Search AND LotID = @LotID AND ParkingStall.ParkingStallTypeID = @typeId AND isActive = 1
RETURN 0
