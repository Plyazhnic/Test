ALTER TABLE [UserProfile].[UserProfile]
    ADD CONSTRAINT [FK_UserProfile_UserProfileType] 
	FOREIGN KEY ([UserProfileTypeID]) 
	REFERENCES [UserProfile].[UserProfileType] ([UserProfileTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

