CREATE PROCEDURE [UserProfile].[Usp_ChangeVehicleMake]
	@IsActive bit,
	@Name varchar(50),
	@Description varchar(1024),
	@Id int
AS
	UPDATE [UserProfile].[VehicleMake] SET isActive = @IsActive, VehicleMake = @Name, VehicleMakeDescription = @Description, UpdatedDate = GETDATE()
	WHERE VehicleMakeID = @Id 
RETURN 0
