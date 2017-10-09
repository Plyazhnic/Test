CREATE TABLE [UserProfile].[UserProfilePreferences] (
    [ProfilePreferencesID] INT            IDENTITY (1, 1) NOT NULL,
    [UserProfileID]        INT            NULL,
    [ProfilePreference]    VARCHAR (128)  NULL,
    [PreferenceValue]      VARCHAR (1024) NULL,
    [CreatedDate]          DATETIME       NULL
);

