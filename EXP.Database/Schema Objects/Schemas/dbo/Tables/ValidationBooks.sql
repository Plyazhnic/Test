CREATE TABLE [dbo].[ValidationBooks]
(
	[BookID] INT IDENTITY(1,1) NOT NULL, 
    [LotID] INT NOT NULL, 
    [CreatedDate] DATETIME NULL, 
    [UpdatedDate] DATETIME NULL, 
    [IsActive] BIT NOT NULL, 
    [BookName] VARCHAR(50) NULL, 
    [TicketCount] INT NULL, 
    [Rate] DECIMAL(6, 2) NULL, 
)
