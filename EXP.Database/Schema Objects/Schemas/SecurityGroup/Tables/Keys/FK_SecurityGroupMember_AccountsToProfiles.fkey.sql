ALTER TABLE [SecurityGroup].[SecurityGroupMember]
    ADD CONSTRAINT [FK_SecurityGroupMember_AccountsToProfiles] FOREIGN KEY ([AccountToProfileID]) REFERENCES [dbo].[AccountsToProfiles] ([AccountToProfileID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

