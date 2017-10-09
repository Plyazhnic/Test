ALTER TABLE [SecurityGroup].[SecurityRolePermission]
    ADD CONSTRAINT [FK_SecurityRolePermission_SecurityRole] FOREIGN KEY ([SecurityRoleID]) REFERENCES [SecurityGroup].[SecurityRole] ([SecurityRoleID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

