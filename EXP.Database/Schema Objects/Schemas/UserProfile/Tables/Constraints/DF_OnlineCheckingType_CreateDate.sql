ALTER TABLE [UserProfile].[OnlineCheckingType]
	ADD CONSTRAINT [DF_OnlineCheckingType_CreateDate] DEFAULT (getdate()) FOR [CreatedDate]
