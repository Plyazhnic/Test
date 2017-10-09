ALTER TABLE [UserProfile].[UserProfileType]
    ADD CONSTRAINT [DF_UserProfileType_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

