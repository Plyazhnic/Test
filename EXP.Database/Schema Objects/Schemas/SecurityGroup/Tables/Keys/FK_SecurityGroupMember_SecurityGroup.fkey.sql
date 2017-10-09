ALTER TABLE [SecurityGroup].[SecurityGroupMember]
    ADD CONSTRAINT [FK_SecurityGroupMember_SecurityGroup] FOREIGN KEY ([SecurityGroupID]) REFERENCES [SecurityGroup].[SecurityGroup] ([SecurityGroupID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

