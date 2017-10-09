CREATE PROCEDURE [dbo].[usp_SearchBuildings]
	@BuildingName nvarchar(max),
	@OwnersFirstName nvarchar(max),
	@OwnersLastName nvarchar(max),
	@ManagerFirstName nvarchar(max),
	@ManagerLastName nvarchar(max),
	@City nvarchar(max),
	@StateID int,
	@ZipCode nvarchar(max)
AS

	select	Building.BuildingID, Building.BuildingName, Address.City, Building.IsActive,
			buildingOwner.FirstName as OwnerFirstName, buildingOwner.LastName as OwnerLastName,
			buildingManager.FirstName as ManagerFirstName, buildingManager.LastName as ManagerLastName
	from Building
	inner join UserProfile.UserProfile as buildingOwner on Building.OwnerID = buildingOwner.UserProfileID
	inner join UserProfile.UserProfile as buildingManager on Building.ManagerID = buildingManager.UserProfileID
	inner join UserProfile.Address on Building.AddressID = Address.AddressID
	where (@BuildingName is null or Building.BuildingName like '%' + @BuildingName + '%')
	and (@OwnersFirstName is null or buildingOwner.FirstName like '%' + @OwnersFirstName + '%')
	and (@OwnersLastName is null or buildingOwner.LastName like '%' + @OwnersLastName + '%')
	and (@ManagerFirstName is null or buildingManager.FirstName like '%' + @ManagerFirstName + '%')
	and (@ManagerLastName is null or buildingManager.LastName like '%' + @ManagerLastName + '%')
	and (@City is null or Address.City like '%' + @City + '%')
	and (@StateID is null or Address.StateID = @StateID)
	and (@ZipCode is null or Address.ZipCode like '%' + @ZipCode + '%')

RETURN 0