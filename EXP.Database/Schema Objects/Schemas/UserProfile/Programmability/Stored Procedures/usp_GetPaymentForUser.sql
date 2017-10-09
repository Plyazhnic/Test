CREATE PROCEDURE [UserProfile].[usp_GetPaymentForUser]
	@UserProfileID int 

AS
	DECLARE @isCreditCard bit;
--	SET @isCreditCard = 
    
	IF EXISTS (SELECT Payment.isCreditCard FROM UserProfile.Payment WHERE  UserProfileID=@UserProfileID AND isCreditCard = 1)
		SELECT TOP 1 UserProfile.Payment.*, UserProfile.CreditCard.*, UserProfile.[Address].*
		FROM [UserProfile].[Payment]
		LEFT JOIN UserProfile.CreditCard ON UserProfile.Payment.CreditCardID = UserProfile.CreditCard.CreditCardID
		LEFT JOIN UserProfile.[Address] ON UserProfile.CreditCard.AddressID = UserProfile.[Address].AddressID
		WHERE  UserProfileID=@UserProfileID
	ELSE
		SELECT TOP 1 UserProfile.Payment.*, UserProfile.OnlineCheck.*
		FROM [UserProfile].[Payment]
		LEFT JOIN UserProfile.OnlineCheck ON UserProfile.Payment.OnlineCheckID = UserProfile.OnlineCheck.OnlineCheckID
		WHERE  UserProfileID=@UserProfileID
RETURN 0
