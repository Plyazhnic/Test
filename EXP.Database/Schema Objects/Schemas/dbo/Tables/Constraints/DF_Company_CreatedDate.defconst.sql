ALTER TABLE [dbo].[Company]
    ADD CONSTRAINT [DF_Company_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

