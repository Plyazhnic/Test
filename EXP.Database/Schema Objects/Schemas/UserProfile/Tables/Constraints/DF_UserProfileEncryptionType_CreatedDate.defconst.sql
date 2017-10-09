ALTER TABLE [UserProfile].[UserProfileEncryptionType] 
    ADD CONSTRAINT [DF_UserProfileEncryptionType_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];
