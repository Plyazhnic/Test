ALTER TABLE [UserProfile].[PhonesToProfiles]
	ADD CONSTRAINT [FK_PhonesToProfiles_UserProfile]
	FOREIGN KEY ([UserProfileID]) REFERENCES [UserProfile].[UserProfile] ([UserProfileID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
