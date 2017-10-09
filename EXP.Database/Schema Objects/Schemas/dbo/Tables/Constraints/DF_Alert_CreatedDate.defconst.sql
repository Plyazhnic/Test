ALTER TABLE [dbo].[Alert]
    ADD CONSTRAINT [DF_Alert_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

