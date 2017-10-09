CREATE PROCEDURE [dbo].[usp_GetLease]
	@LeaseID int

AS
	SELECT TOP(1) Lease.*, Building.*, Company.*, [Address].*, zState.*
    FROM [dbo].[Lease]
    INNER JOIN dbo.Building ON dbo.Building.BuildingID = dbo.Lease.BuildingID
    INNER JOIN dbo.Company ON dbo.Company.CompanyID = dbo.Lease.CompanyID
    INNER JOIN [UserProfile].[Address] ON dbo.Building.AddressID = [UserProfile].[Address].AddressID
	  INNER JOIN dbo.[zState] ON dbo.[zState].StateID = [UserProfile].[Address].StateID
    WHERE dbo.Lease.LeaseID = @LeaseID
GO

