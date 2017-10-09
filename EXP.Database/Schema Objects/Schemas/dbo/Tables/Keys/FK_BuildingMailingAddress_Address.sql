ALTER TABLE [dbo].[Building]
	ADD CONSTRAINT [FK_BuildingMailingAddress_Address]
	FOREIGN KEY ([MailingAddressID]) REFERENCES [UserProfile].[Address] ([AddressID]) ON DELETE NO ACTION ON UPDATE NO ACTION;