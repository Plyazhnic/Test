ALTER TABLE [UserProfile].[VehicleModel]
	ADD CONSTRAINT [FK_VehicleModel_VehicleMake] FOREIGN KEY ([VehicleModelID]) REFERENCES [UserProfile].[VehicleMake] ([VehicleMakeID])	ON DELETE NO ACTION ON UPDATE NO ACTION;

