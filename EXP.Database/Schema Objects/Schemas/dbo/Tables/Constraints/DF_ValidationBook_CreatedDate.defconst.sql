ALTER TABLE [dbo].[ValidationBooks]
    ADD CONSTRAINT [DF_ValidationBook_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];
