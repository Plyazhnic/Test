ALTER TABLE [SecurityGroup].[SecurityGroupMember]
    ADD CONSTRAINT [FK_SecurityGroupMember_SecurityRole] FOREIGN KEY ([SecurityRoleID]) REFERENCES [SecurityGroup].[SecurityRole] ([SecurityRoleID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

