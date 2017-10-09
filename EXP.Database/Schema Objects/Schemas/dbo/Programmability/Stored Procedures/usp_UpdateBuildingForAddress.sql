CREATE PROCEDURE [dbo].[usp_UpdateBuildingForAddress]
    @BuildingID int,
	@BuildingName varchar(256),
	@Address1 varchar(256),
	@City varchar(50),
	@StateID int,
	@ZipCode varchar(10)
AS
	UPDATE [dbo].[Building] 
    SET BuildingName = @BuildingName,
	UpdatedDate = GETDATE()
    WHERE BuildingID = @BuildingID
    
	UPDATE A
    SET A.Address1 = @Address1,
	A.City = @City,
	A.StateID = @StateID,
	A.ZipCode = @ZipCode 
	    
	FROM [dbo].[Building] B
	INNER JOIN  [UserProfile].[Address] A ON B.AddressID = A.AddressID 
	
	WHERE B.BuildingID = @BuildingID 

RETURN 0
