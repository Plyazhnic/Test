CREATE PROCEDURE [UserProfile].[usp_UpdateAddress]
	@AddressID int,
	@City varchar(50),
	@Address1 varchar(256),
	@Address2 varchar(256),
	@StateID int,
	@ZipCode varchar(10)
	 
AS
	UPDATE [UserProfile].[Address]
    SET City = @City,
    Address1 = @Address1,
    Address2 = @Address2,
    StateID = @StateID,
    ZipCode = @ZipCode

    WHERE AddressID = @AddressID

RETURN 0
