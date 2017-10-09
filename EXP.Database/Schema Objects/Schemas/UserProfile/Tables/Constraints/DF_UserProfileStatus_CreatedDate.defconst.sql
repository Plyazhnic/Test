ALTER TABLE [UserProfile].[UserProfileStatus]
    ADD CONSTRAINT [DF_UserProfileStatus_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

