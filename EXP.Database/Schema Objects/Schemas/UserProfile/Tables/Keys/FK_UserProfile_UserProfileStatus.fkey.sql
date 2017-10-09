ALTER TABLE [UserProfile].[UserProfile]
    ADD CONSTRAINT [FK_UserProfile_UserProfileStatus] FOREIGN KEY ([UserProfileStatusID]) 
	REFERENCES [UserProfile].[UserProfileStatus] ([UserProfileStatusID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

