CREATE TABLE [dbo].[KeyCards]
(
	[KeyCardID] INT IDENTITY(1,1) NOT NULL , 
    [LotID] INT NOT NULL, 
    [KeyCardName] NCHAR(50) NULL, 
    [Rate] DECIMAL(6, 2) NULL, 
    [CreatedDate] DATETIME NULL, 
    [UpdatedDate] DATETIME NULL, 
    [IsActive] BIT NOT NULL
)
