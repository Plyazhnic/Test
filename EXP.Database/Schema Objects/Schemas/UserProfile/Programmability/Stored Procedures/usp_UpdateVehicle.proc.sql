CREATE PROCEDURE [UserProfile].[usp_UpdateVehicle]
    @VehicleID int,
    @UserProfileID int,
    @Year int,
    @VehicleMakeID int,
    @VehicleModelID int,
    @Color varchar(32),
    @LicensePlateNumber  varchar(32),
    @PermitNumber  varchar(32)
AS
    UPDATE UserProfile.Vehicle
    SET [Year] = @Year,
    VehicleMakeID = @VehicleMakeID,
    VehicleModelID = @VehicleModelID,
    Color = @Color,
    LicensePlateNumber = @LicensePlateNumber,
    PermitNumber = @PermitNumber,
    UpdatedDate = GETDATE()
    WHERE VehicleID = @VehicleID AND UserProfileID = @UserProfileID

RETURN 0