ALTER TABLE [dbo].[Building]
    ADD CONSTRAINT [DF_Building_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

