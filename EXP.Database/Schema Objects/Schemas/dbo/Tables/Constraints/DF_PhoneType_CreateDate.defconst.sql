ALTER TABLE [dbo].[PhoneType]
   ADD CONSTRAINT [DF_PhoneType_CreateDate] DEFAULT (getdate()) FOR [CreatedDate];


