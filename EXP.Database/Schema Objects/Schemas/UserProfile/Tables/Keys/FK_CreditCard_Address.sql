ALTER TABLE [UserProfile].[CreditCard]
	ADD CONSTRAINT [FK_CreditCard_Address]
	FOREIGN KEY ([AddressID]) REFERENCES [UserProfile].[Address] ([AddressID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
