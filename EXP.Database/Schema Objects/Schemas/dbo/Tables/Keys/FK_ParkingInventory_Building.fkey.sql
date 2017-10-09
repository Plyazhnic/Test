ALTER TABLE [dbo].[ParkingInventory]
    ADD CONSTRAINT [FK_ParkingInventory_Building] FOREIGN KEY ([BuildingID]) REFERENCES [dbo].[Building] ([BuildingID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

