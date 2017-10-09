ALTER TABLE [UserProfile].[UserProfileToCompany]
	ADD CONSTRAINT [FK_UserProfileToCompany_UserProfile]
	FOREIGN KEY ([UserProfileID]) REFERENCES [UserProfile].[UserProfile] ([UserProfileID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
