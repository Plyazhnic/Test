ALTER TABLE [UserProfile].[UserProfile]
	ADD CONSTRAINT [FK_UserProfile_Address]
	FOREIGN KEY ([AddressID]) 
	REFERENCES [UserProfile].[Address] ([AddressID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
