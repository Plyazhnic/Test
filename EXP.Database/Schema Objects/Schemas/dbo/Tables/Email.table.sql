CREATE TABLE [dbo].[Email] (
    [EmailID]              INT            IDENTITY (1, 1) NOT NULL,
    [Subject]              NVARCHAR(MAX)  NOT NULL,
    [Body]                 NVARCHAR(MAX)  NOT NULL,
    [Destination]          NVARCHAR(MAX)  NOT NULL,
    [Sent]	               BIT            NOT NULL default (0),
    [SentDate]             DATETIME       NULL,
    [CreatedDate]          DATETIME       NOT NULL default(getdate())
);