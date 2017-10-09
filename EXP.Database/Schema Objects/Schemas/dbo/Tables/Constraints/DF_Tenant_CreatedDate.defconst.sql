ALTER TABLE [dbo].[Tenant]
    ADD CONSTRAINT [DF_Tenant_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

