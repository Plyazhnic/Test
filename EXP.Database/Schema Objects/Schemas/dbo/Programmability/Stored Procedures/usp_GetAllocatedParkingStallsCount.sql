CREATE PROCEDURE [dbo].[usp_GetAllocatedParkingStallsCount]
	@Search varchar(50),
	@ParkingStallID int

	AS

	SELECT @Search = '%' + RTRIM(@Search) + '%';

	SELECT count(*) --[dbo].[ParkingInventory].*, dbo.[Company].CompanyName,
	FROM [dbo].[ParkingInventory]
	LEFT JOIN dbo.[Company] ON Company.CompanyID = ParkingInventory.CompanyID 
	WHERE Company.CompanyName LIKE @Search AND ParkingInventory.ParkingStallID = @ParkingStallID

RETURN 0
