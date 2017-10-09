ALTER TABLE [dbo].[ParkingInventory]
    ADD CONSTRAINT [FK_ParkingInventory_ParkingStall] FOREIGN KEY ([ParkingStallID]) REFERENCES [dbo].[ParkingStall] ([ParkingStallID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

