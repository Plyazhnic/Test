ALTER TABLE [dbo].[Company]
	ADD CONSTRAINT [FK_Company_Address]
	FOREIGN KEY ([AddressID]) REFERENCES [UserProfile].[Address] ([AddressID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
