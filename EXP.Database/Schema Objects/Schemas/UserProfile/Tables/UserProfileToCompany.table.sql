CREATE TABLE [UserProfile].[UserProfileToCompany]
(
	[UserProfileToCompanyID] INT NOT NULL, 
    [UserProfileID] INT NOT NULL, 
    [CompanyID] INT NOT NULL,
	[isActive]           BIT      NOT NULL,
    [UpdatedDate]        DATETIME NULL,
    [CreatedDate]        DATETIME NOT NULL
)
