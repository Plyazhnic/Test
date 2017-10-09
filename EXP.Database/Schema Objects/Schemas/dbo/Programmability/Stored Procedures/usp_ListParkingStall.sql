CREATE PROCEDURE [dbo].[usp_ListParkingStall]
	
AS
	SELECT * FROM [dbo].[ParkingStall] WHERE isActive = 1
RETURN 0
