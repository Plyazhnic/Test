ALTER TABLE [UserProfile].[OnlineCheck]
	ADD CONSTRAINT [DF__CreateDate] DEFAULT (getdate()) FOR [CreatedDate]
