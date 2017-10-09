ALTER TABLE [UserProfile].[UserProfilePreferences]
    ADD CONSTRAINT [DF_ProfilePreferences_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

