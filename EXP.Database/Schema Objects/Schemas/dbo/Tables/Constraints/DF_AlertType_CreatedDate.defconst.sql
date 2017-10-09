ALTER TABLE [dbo].[AlertType]
    ADD CONSTRAINT [DF_AlertType_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

