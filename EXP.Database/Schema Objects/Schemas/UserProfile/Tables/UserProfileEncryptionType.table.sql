CREATE TABLE [UserProfile].[UserProfileEncryptionType](
	[UserProfileEncryptionTypeID] [int] IDENTITY(1,1) NOT NULL,
	[UserProfileEncryptionType] [varchar](128) NOT NULL,
	[UserProfileEncryptionTypeDescription] [varchar](1024) NOT NULL,
	[isActive] [bit] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_UserProfileEncryptionType] PRIMARY KEY CLUSTERED 
(
	[UserProfileEncryptionTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
) ON [PRIMARY]
