CREATE TABLE [UserProfile].[PhonesToProfiles]
(
	[PhonesToProfilesID]   INT      IDENTITY (1, 1) NOT NULL,
    [PhoneID]            INT      NOT NULL,
    [UserProfileID]      INT      NOT NULL,
    [isActive]           BIT      NOT NULL,
    [UpdatedDate]        DATETIME NULL,
    [CreatedDate]        DATETIME NOT NULL
)
