ALTER TABLE [dbo].[Account]
    ADD CONSTRAINT [DF_Account_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

