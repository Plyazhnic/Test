ALTER TABLE [dbo].[Account]
    ADD CONSTRAINT [FK_Account_AccountType] FOREIGN KEY ([AccountTypeID]) REFERENCES [dbo].[AccountType] ([AccountTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

