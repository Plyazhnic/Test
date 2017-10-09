ALTER TABLE [UserProfile].[Vehicle]
    ADD CONSTRAINT [FK_Vehicle_VehicleModel] FOREIGN KEY ([VehicleModelID]) REFERENCES [UserProfile].[VehicleModel] ([VehicleModelID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

