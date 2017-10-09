CREATE PROCEDURE [UserProfile].[usp_GetMake]
    @Make varchar(256)
AS
    SELECT TOP 1 * From [UserProfile].[VehicleMake] WHERE VehicleMake = @Make

RETURN 0