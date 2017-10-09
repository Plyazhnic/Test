ALTER TABLE [UserProfile].[Vehicle]
    ADD CONSTRAINT [FK_Vehicle_VehicleMake] FOREIGN KEY ([VehicleMakeID]) REFERENCES [UserProfile].[VehicleMake] ([VehicleMakeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

