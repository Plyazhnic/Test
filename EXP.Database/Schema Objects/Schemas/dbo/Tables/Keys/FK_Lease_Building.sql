ALTER TABLE [dbo].[Lease]
	ADD CONSTRAINT [FK_Lease_Building]
	FOREIGN KEY ([BuildingID]) REFERENCES [dbo].[Building] ([BuildingID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
