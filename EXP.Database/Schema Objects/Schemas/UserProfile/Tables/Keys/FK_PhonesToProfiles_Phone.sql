ALTER TABLE [UserProfile].[PhonesToProfiles]
	ADD CONSTRAINT [FK_PhonesToProfiles_Phone]
	FOREIGN KEY ([PhoneID]) REFERENCES [dbo].[Phone] ([PhoneID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
