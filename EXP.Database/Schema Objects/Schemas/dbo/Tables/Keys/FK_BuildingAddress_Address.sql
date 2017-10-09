ALTER TABLE [dbo].[Building]
	ADD CONSTRAINT [FK_BuildingAddress_Address]
	FOREIGN KEY ([AddressID]) REFERENCES [UserProfile].[Address] ([AddressID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
