CREATE PROCEDURE [UserProfile].[usp_UpdateOnlineCheck]
	@OnlineCheckID int,
	@NameOnAccount varchar(50),
    @OnlineCheckingTypeID int,
    @BankName varchar(128),
    @RoutingNumber varchar(20),
    @CheckingAccountNumber varchar(50)
AS
	UPDATE [UserProfile].[OnlineCheck]
   SET [NameOnAccount] = @NameOnAccount
      ,[OnlineCheckingTypeID] = @OnlineCheckingTypeID
      ,[BankName] = @BankName
      ,[RoutingNumber] = @RoutingNumber
      ,[CheckingAccountNumber] = @CheckingAccountNumber
      ,[UpdatedDate] = GETDATE()

 WHERE OnlineCheckID = @OnlineCheckID

RETURN 0
