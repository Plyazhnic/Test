ALTER TABLE [dbo].[KeyCards]
    ADD CONSTRAINT [DF_KeyCard_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];