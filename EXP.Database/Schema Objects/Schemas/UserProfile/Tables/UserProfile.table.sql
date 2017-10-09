CREATE TABLE [UserProfile].[UserProfile] (
    [UserProfileID]					INT            IDENTITY (1, 1) NOT NULL,
    [UserProfileTypeID]				INT            NOT NULL,
    [UserProfileEncryptionTypeID]	INT            NOT NULL,
	[UserProfileStatusID]			TINYINT        NOT NULL,
	[SessionID]				        VARCHAR (32)   NULL,
	[TenantID]				        INT            NULL,
	[OperatorID]				    INT            NULL,
    [LoginName]						NCHAR (10)     NOT NULL,
    [ProfilePassword]				VARCHAR (1024) NOT NULL,
	[ProfilePasswordSalt]			VARCHAR (1024) NOT NULL,
    [EmailAddress]					VARCHAR (256)  NOT NULL,
    [TitlePreffix]					CHAR (4)       NULL,
    [TitleSuffix]					CHAR (4)       NULL,
    [FirstName]						VARCHAR (256)  NOT NULL,
    [LastName]						VARCHAR (256)  NOT NULL,
	[AddressID]						INT			   NULL,
    [Comments]						VARCHAR (1024) NULL,
	[isSupervisor]					bit			   NULL,
	[HireDate]						DATE		   NULL,
    [AgreementVersion]				VARCHAR (50)   NOT NULL,
    [AgreementDate]					DATETIME       NOT NULL,
    [LastLoginDate]					DATETIME       NOT NULL,
    [UpdateDate]					DATETIME       NULL,
    [CreateDate]					DATETIME       NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SK', @level0type = N'SCHEMA', @level0name = N'UserProfile', @level1type = N'TABLE', @level1name = N'UserProfile', @level2type = N'COLUMN', @level2name = N'UserProfileID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK', @level0type = N'SCHEMA', @level0name = N'UserProfile', @level1type = N'TABLE', @level1name = N'UserProfile', @level2type = N'COLUMN', @level2name = N'UserProfileTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK', @level0type = N'SCHEMA', @level0name = N'UserProfile', @level1type = N'TABLE', @level1name = N'UserProfile', @level2type = N'COLUMN', @level2name = N'UserProfileEncryptionTypeID';
