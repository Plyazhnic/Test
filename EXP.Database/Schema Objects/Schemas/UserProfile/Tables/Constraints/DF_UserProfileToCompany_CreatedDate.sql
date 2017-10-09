ALTER TABLE [UserProfile].[UserProfileToCompany]
	ADD CONSTRAINT [DF_UserProfileToCompany_CreateDate] DEFAULT (getdate()) FOR [CreatedDate];