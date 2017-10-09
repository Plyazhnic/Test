CREATE PROCEDURE [UserProfile].[usp_DeleteAddress]
	@AddressID int
AS
	DELETE FROM [UserProfile].[Address]
      WHERE AddressID = @AddressID

RETURN 0
