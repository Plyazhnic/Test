ALTER TABLE [dbo].[ParkingInventory]
    ADD CONSTRAINT [FK_ParkingInventory_Tenant] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([CompanyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

