CREATE TABLE [UserProfile].[OnlineCheckingType]
(
	[OnlineCheckingTypeID] INT IDENTITY (1, 1) NOT NULL, 
    [OnlineCheckingType] VARCHAR(50) NOT NULL, 
    [OnlineCheckingTypeDescription] VARCHAR(128) NULL, 
    [isActive] BIT NOT NULL,
	[UpdatedDate] DATETIME NULL,
	[CreatedDate] DATETIME NOT NULL
)
