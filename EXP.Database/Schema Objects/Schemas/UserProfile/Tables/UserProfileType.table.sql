CREATE TABLE [UserProfile].[UserProfileType] (
    [UserProfileTypeID]          INT            IDENTITY (1, 1) NOT NULL,
    [UserProfileType]             VARCHAR (128)  NOT NULL,
    [UserProfileTypeDescription] VARCHAR (1024) NOT NULL,
    [isActive]                   BIT            NOT NULL,
    [UpdatedDate]                DATETIME       NULL,
    [CreatedDate]                DATETIME       NOT NULL
);

