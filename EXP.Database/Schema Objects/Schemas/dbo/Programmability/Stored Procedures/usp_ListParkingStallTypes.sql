CREATE PROCEDURE [dbo].[usp_ListParkingStallTypes]
	
AS
	SELECT * FROM dbo.ParkingStallType WHERE isActive = 1

RETURN 0
