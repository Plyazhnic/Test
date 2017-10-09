CREATE PROCEDURE [UserProfile].[usp_CreatePaymentOnlineCheck]
	@NameOnAccount varchar(50),
    @OnlineCheckingTypeID int,
    @BankName varchar(128),
    @RoutingNumber varchar(20),
    @CheckingAccountNumber varchar(50),
	@UserProfileID int,
	@isCreditCard bit
AS
	INSERT INTO [UserProfile].[OnlineCheck]
           ([NameOnAccount]
           ,[OnlineCheckingTypeID]
           ,[BankName]
           ,[RoutingNumber]
           ,[CheckingAccountNumber]
           ,[UpdatedDate]
           ,[CreatedDate])
     VALUES
           (@NameOnAccount,
           @OnlineCheckingTypeID,
           @BankName,
           @RoutingNumber,
           @CheckingAccountNumber,
           GETDATE(),
           GETDATE())

	INSERT INTO [UserProfile].[Payment]
           ([UserProfileID]
           ,[isCreditCard]
           ,[OnlineCheckID])
     VALUES
           (@UserProfileID,
           @isCreditCard,
           Scope_Identity())
RETURN 0
