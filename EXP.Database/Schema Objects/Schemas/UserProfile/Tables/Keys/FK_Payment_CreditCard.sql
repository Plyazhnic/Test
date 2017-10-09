ALTER TABLE [UserProfile].[Payment]
	ADD CONSTRAINT [FK_Payment_CreditCard] FOREIGN KEY ([CreditCardID]) 
	REFERENCES [UserProfile].[CreditCard] ([CreditCardID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
