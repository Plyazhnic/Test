ALTER TABLE [dbo].[AccountType]
    ADD CONSTRAINT [DF_AccountType_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

