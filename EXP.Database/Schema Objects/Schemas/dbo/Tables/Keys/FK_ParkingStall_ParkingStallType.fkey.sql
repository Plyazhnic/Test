ALTER TABLE [dbo].[ParkingStall]
    ADD CONSTRAINT [FK_ParkingStall_ParkingStallType] FOREIGN KEY ([ParkingStallTypeID]) REFERENCES [dbo].[ParkingStallType] ([ParkingStallTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

