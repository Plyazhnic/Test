CREATE TABLE [dbo].[CompanyToProfiles]
(
	[CompanyToProfilesID] INT IDENTITY (1, 1) NOT NULL, 
	[CompanyID]			  INT NOT NULL, 
    [UserProfileID]       INT NOT NULL, 
)
