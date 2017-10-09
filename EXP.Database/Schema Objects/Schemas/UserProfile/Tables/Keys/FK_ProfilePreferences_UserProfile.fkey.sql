ALTER TABLE [UserProfile].[UserProfilePreferences]
    ADD CONSTRAINT [FK_ProfilePreferences_UserProfile] 
	FOREIGN KEY ([UserProfileID]) 
	REFERENCES [UserProfile].[UserProfile] ([UserProfileID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

