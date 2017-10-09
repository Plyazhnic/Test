ALTER TABLE [UserProfile].[OnlineCheck]
	ADD CONSTRAINT [FK_OnlineCheck_OnlineCheckingType] FOREIGN KEY ([OnlineCheckingTypeID]) 
	REFERENCES [UserProfile].[OnlineCheckingType] ([OnlineCheckingTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
