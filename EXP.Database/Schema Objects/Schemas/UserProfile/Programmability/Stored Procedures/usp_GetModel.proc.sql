CREATE PROCEDURE [UserProfile].[usp_GetModel]
    @Model varchar(256)
AS
    SELECT TOP 1 * From [UserProfile].[VehicleModel] WHERE VehicleModel = @Model

RETURN 0