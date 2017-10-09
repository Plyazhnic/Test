ALTER TABLE [dbo].[AlertEntityType]
   ADD CONSTRAINT [DF_AlertEntityType_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];



