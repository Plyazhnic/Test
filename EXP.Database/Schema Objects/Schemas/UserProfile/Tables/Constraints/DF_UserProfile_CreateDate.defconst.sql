ALTER TABLE [UserProfile].[UserProfile]
    ADD CONSTRAINT [DF_UserProfile_CreateDate] DEFAULT (getdate()) FOR [CreateDate];

