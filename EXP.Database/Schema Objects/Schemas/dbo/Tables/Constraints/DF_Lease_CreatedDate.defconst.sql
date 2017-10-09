ALTER TABLE [dbo].[Lease]
    ADD CONSTRAINT [DF_Lease_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

