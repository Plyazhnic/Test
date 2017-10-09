CREATE PROCEDURE [UserProfile].[usp_UpdateCreditCard]
	@CreditCardID int,
	@CHFirstName varchar(50),
	@CHLastName varchar(50),
	@CardNumber varchar(20),
	@ExpDateMount varchar(2),
	@ExpDateYear varchar(4),
	@CVV varchar(4),
	@AutoPay bit,
	@AddressID int,
	@isHome bit
AS
	UPDATE [UserProfile].[CreditCard]
	SET [CHFirstName] = @CHFirstName
      ,[CHLastName] = @CHLastName
      ,[CardNumber] = @CardNumber
      ,[ExpDateMount] = @ExpDateMount
      ,[ExpDateYear] = @ExpDateYear
      ,[CVV] = @CVV
      ,[AutoPay] = @AutoPay
      ,[AddressID] = @AddressID
	  ,[isHome] = @isHome
      ,[UpdatedDate] = GETDATE()
 
 WHERE CreditCardID = @CreditCardID

RETURN 0
