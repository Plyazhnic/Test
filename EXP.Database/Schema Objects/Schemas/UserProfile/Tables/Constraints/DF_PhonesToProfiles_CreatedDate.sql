ALTER TABLE [UserProfile].[PhonesToProfiles]
	ADD CONSTRAINT [DF_PhonesToProfiles_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];
