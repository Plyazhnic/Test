ALTER TABLE [dbo].[ParkingInventory]
    ADD CONSTRAINT [DF_ParkingInventory_EffectiveFrom] DEFAULT (getdate()) FOR [EffectiveFrom];

