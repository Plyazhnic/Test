CREATE TABLE [UserProfile].[Payment]
(
	[PaymentID] INT IDENTITY (1, 1) NOT NULL, 
    [UserProfileID] INT NOT NULL, 
    [isCreditCard] BIT NOT NULL, 
    [CreditCardID] INT NULL, 
    [OnlineCheckID] INT NULL 
)
