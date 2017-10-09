ALTER TABLE [dbo].[ParkingInventory]
    ADD CONSTRAINT [FK_ParkingInventory_ParkingStallType] FOREIGN KEY ([ParkingStallTypeID]) REFERENCES [dbo].[ParkingStallType] ([ParkingStallTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

