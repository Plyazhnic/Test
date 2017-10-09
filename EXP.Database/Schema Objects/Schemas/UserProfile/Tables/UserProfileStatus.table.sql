CREATE TABLE [UserProfile].[UserProfileStatus] (
    [UserProfileStatusID]          TINYINT        IDENTITY (1, 1) NOT NULL,
    [UserProfileStatus]            VARCHAR (32)   NULL,
    [UserProfileStatusDescription] VARCHAR (1024) NULL,
    [CreatedDate]                  DATETIME       NULL
);

