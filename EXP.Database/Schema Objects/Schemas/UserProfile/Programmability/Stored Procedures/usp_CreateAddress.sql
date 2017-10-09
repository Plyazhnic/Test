CREATE PROCEDURE [UserProfile].[usp_CreateAddress]
	@City varchar(50),
	@Address1 varchar(256),
	@Address2 varchar(256),
	@StateID int,
	@ZipCode varchar(10),
	@NewAddressID int out

AS
	INSERT INTO [UserProfile].[Address]
           ([City]
           ,[Address1]
           ,[Address2]
           ,[StateID]
           ,[ZipCode]
           ,[UpdatedDate]
           ,[CreatedDate])
     VALUES
           (@City,
           @Address1,
           @Address2,
           @StateID,
           @ZipCode,
           NULL,
           GETDATE());

		   SET @NewAddressID = Scope_Identity();

	RETURN 1