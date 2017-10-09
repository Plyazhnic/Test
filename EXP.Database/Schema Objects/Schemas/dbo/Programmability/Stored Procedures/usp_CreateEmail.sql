CREATE PROCEDURE [dbo].[usp_CreateEmail]
	@Subject nvarchar(max),
	@Body nvarchar(max),
	@Destination nvarchar(max)
AS
	INSERT INTO [dbo].[Email] ([Subject], [Body] ,[Destination])
    VALUES (@Subject ,@Body ,@Destination)

RETURN 0
