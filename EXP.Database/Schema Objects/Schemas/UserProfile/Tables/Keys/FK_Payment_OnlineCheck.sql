ALTER TABLE [UserProfile].[Payment]
	ADD CONSTRAINT [FK_Payment_OnlineCheck] FOREIGN KEY ([OnlineCheckID]) 
	REFERENCES [UserProfile].[OnlineCheck] ([OnlineCheckID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
