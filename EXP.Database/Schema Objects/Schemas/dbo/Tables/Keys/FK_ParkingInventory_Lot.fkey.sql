ALTER TABLE [dbo].[ParkingInventory]
    ADD CONSTRAINT [FK_ParkingInventory_Lot] FOREIGN KEY ([LotID]) REFERENCES [dbo].[Lot] ([LotID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

