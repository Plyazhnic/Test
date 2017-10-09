ALTER TABLE [UserProfile].[Vehicle]
    ADD CONSTRAINT [DF_Vehicle_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

