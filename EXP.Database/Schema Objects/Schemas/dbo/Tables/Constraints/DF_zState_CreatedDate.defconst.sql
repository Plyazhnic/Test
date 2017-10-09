ALTER TABLE [dbo].[zState]
    ADD CONSTRAINT [DF_zState_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

