ALTER TABLE [SecurityGroup].[SecurityRolePermission]
    ADD CONSTRAINT [DF_SecurityRolePermission_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];

