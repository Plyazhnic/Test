ALTER TABLE [UserProfile].[Address]
	ADD CONSTRAINT [DF_Address_CreateDate] DEFAULT (getdate()) FOR [CreatedDate];