ALTER TABLE [UserProfile].[CreditCard]
	ADD CONSTRAINT [DF_CreditCard_CreateDate] DEFAULT (getdate()) FOR [CreatedDate];
