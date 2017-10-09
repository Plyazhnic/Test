CREATE PROCEDURE [UserProfile].[usp_CreatePaymentCreditCard]
	@CHFirstName varchar(50),
	@CHLastName varchar(50),
	@CardNumber varchar(20),
	@ExpDateMount varchar(2),
	@ExpDateYear varchar(4),
	@CVV varchar(4),
	@AutoPay bit,
	@AddressID int,
	@isHome bit,
	@UserProfileID int,
	@isCreditCard bit

AS
	INSERT INTO [UserProfile].[CreditCard]
           ([CHFirstName]
           ,[CHLastName]
           ,[CardNumber]
           ,[ExpDateMount]
           ,[ExpDateYear]
           ,[CVV]
           ,[AutoPay]
           ,[AddressID]
		   ,[isHome]
           ,[UpdatedDate]
           ,[CreatedDate])
     VALUES
           (@CHFirstName,
           @CHLastName,
           @CardNumber,
           @ExpDateMount,
           @ExpDateYear,
           @CVV,
           @AutoPay,
           @AddressID,
		   @isHome,
           GETDATE(),
           GETDATE());

	INSERT INTO [UserProfile].[Payment]
           ([UserProfileID]
           ,[isCreditCard]
           ,[CreditCardID])
     VALUES
           (@UserProfileID,
           @isCreditCard,
           Scope_Identity())
		   
RETURN 0
