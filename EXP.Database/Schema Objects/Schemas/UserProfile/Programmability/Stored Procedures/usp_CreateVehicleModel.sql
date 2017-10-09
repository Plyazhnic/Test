CREATE PROCEDURE [UserProfile].[usp_CreateVehicleModel]
	@IsActive Bit,
	@Name varchar(256),
	@Description varchar(1024),
	@Year int,
	@MakeId int
AS
If Not Exists(select * from [UserProfile].[VehicleModel] where VehicleModel = @Name )
Begin
insert into [UserProfile].[VehicleModel] (VehicleModel, VehicleModelDescription, isActive, UpdatedDate, CreatedDate,VehicleMakeID ) values (@Name, @Description,@IsActive, null, GETDATE(), @MakeId)
End
RETURN 0
