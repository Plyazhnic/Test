CREATE PROCEDURE [UserProfile].[usp_ListMakes]
AS
SELECT VehicleMake, MIN(VehicleMakeID)VehicleMakeID FROM UserProfile.VehicleMake
GROUP BY VehicleMake
RETURN 0