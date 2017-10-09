CREATE PROCEDURE [UserProfile].[usp_CreateVehicle]
	@UserProfileID int,
    @Year int,
    @VehicleMakeID int,
    @VehicleModelID int,
    @Color varchar(32),
    @LicensePlateNumber  varchar(32),
	@PermitNumber  varchar(32),
	@NewVehicleId int out
	
AS
	INSERT INTO [UserProfile].[Vehicle]
            ([UserProfileID]
            ,[Year]
            ,[VehicleMakeID]
            ,[VehicleModelID]
            ,[Color]
            ,[LicensePlateNumber]
			,[PermitNumber]
            ,[isActive]
            ,[CreatedDate]
            ,[UpdatedDate])
     VALUES
            (@UserProfileID,
            @Year,
            @VehicleMakeID,
            @VehicleModelID,
            @Color,
            @LicensePlateNumber,
			@PermitNumber,
            1,
            GETDATE(),
            GETDATE());
			
			SET @NewVehicleId = Scope_Identity();

RETURN 1