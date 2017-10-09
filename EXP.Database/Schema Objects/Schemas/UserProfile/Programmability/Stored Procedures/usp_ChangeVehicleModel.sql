CREATE PROCEDURE [UserProfile].[usp_ChangeVehicleModel]
	@IsActive bit,
	@Name varchar(50),
	@Description varchar(1024),
	@Id int,
	@Year int
AS
	UPDATE [UserProfile].[VehicleModel] SET isActive = @IsActive, VehicleModel = @Name, VehicleModelDescription = @Description, UpdatedDate = GETDATE(), Year1 = @Year
	WHERE VehicleModelID = @Id 
RETURN 0
