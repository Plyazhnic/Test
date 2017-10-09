ALTER TABLE [UserProfile].[UserProfile]
    ADD CONSTRAINT [FK_UserProfile_UserProfileEncryptionType] 
	FOREIGN KEY ([UserProfileEncryptionTypeID]) 
	REFERENCES [UserProfile].[UserProfileEncryptionType] ([UserProfileEncryptionTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


