ALTER TABLE [UserProfile].[UserProfilePhone]
   ADD CONSTRAINT [DF_UserProfilePhone_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];


