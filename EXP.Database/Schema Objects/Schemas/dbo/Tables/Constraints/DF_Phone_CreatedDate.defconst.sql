ALTER TABLE [dbo].[Phone]
	ADD CONSTRAINT [DF_Phone_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

