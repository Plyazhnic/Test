CREATE PROCEDURE [dbo].[usp_ListAllocatedParkingStallForPage]
	@ParkingStallID int,
	@Start int,
	@End int,
	@Search varchar(50)

AS
	SELECT @Search = '%' + RTRIM(@Search) + '%';

	WITH AllocatedStallPage AS
	(
		SELECT [dbo].[ParkingInventory].*, dbo.[Company].CompanyName,
		ROW_NUMBER() OVER (ORDER BY ParkingInventoryID) AS RowNumber
		FROM [dbo].[ParkingInventory]
		LEFT JOIN dbo.[Company] ON Company.CompanyID = ParkingInventory.CompanyID 
		WHERE Company.CompanyName LIKE @Search AND ParkingInventory.ParkingStallID = @ParkingStallID
	)
    
	SELECT AllocatedStallPage.*
	FROM AllocatedStallPage
	WHERE RowNumber > @Start AND RowNumber <= @End

RETURN 0
