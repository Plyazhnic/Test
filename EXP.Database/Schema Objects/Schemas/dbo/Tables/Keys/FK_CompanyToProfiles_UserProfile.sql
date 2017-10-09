ALTER TABLE [dbo].[CompanyToProfiles]
	ADD CONSTRAINT [FK_CompanyToProfiles_UserProfile]
	FOREIGN KEY ([UserProfileID]) REFERENCES [UserProfile].[UserProfile] ([UserProfileID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
