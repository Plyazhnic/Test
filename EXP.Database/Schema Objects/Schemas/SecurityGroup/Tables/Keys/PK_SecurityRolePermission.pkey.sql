ALTER TABLE [SecurityGroup].[SecurityRolePermission]
    ADD CONSTRAINT [PK_SecurityRolePermission] PRIMARY KEY CLUSTERED ([SecurityRolePermissionID] ASC, [SecurityRoleID] ASC, [RolePrivelage] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

