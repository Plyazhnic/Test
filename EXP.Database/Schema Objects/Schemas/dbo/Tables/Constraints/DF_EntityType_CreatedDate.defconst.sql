ALTER TABLE [dbo].[EntityType]
   ADD CONSTRAINT [DF_EntityType_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];



