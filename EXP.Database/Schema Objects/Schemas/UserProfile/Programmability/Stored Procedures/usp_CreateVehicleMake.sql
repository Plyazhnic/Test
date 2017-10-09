CREATE PROCEDURE [UserProfile].[usp_CreateVehicleMake]
	@IsActive bit, 
	@Name varchar(50),
	@Description varchar(50)
AS
If Not Exists(select * from [UserProfile].[VehicleMake] where VehicleMake = @Name )
Begin
insert into [UserProfile].[VehicleMake] (VehicleMake, VehicleMakeDescription, isActive, UpdatedDate, CreatedDate ) values (@Name, @Description,@IsActive, null, GETDATE())
End
RETURN 0
