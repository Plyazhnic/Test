CREATE TABLE [UserProfile].[OnlineCheck]
(
	[OnlineCheckID] INT IDENTITY (1, 1) NOT NULL, 
    [NameOnAccount] VARCHAR(50) NULL, 
    [OnlineCheckingTypeID] INT NOT NULL, 
    [BankName] VARCHAR(128) NULL, 
    [RoutingNumber] VARCHAR(20) NULL, 
    [CheckingAccountNumber] VARCHAR(50) NULL,
	[UpdatedDate] DATETIME NULL,
	[CreatedDate] DATETIME NOT NULL
	
)
