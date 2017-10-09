CREATE TABLE [UserProfile].[CreditCard]
(
	[CreditCardID] INT IDENTITY (1, 1) NOT NULL, 
    [CHFirstName] VARCHAR(50) NULL,
	[CHLastName] VARCHAR(50) NULL, 
    [CardNumber] VARCHAR(20) NULL, 
    [ExpDateMount] VARCHAR(2) NULL,
	[ExpDateYear] VARCHAR(4) NULL, 
    [CVV] VARCHAR(4) NULL, 
    [AutoPay] BIT NULL, 
    [AddressID] INT NULL,
	[isHome] BIT NOT NULL,
    [UpdatedDate] DATETIME NULL,
	[CreatedDate] DATETIME NOT NULL
)
