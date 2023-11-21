create table [dbo].[Login_Log] (
    [Login_LogID]                      INT             identity (1, 1) not null,
    [LogAction]                        CHAR (1)        not null,
    [LogDate]                          DATETIME        not null,
    [LoginID]                          INT             not null,
    [Email]                            NVARCHAR (200)  null,
    [Name]                             NVARCHAR (50)   not null,
    [Password]                         VARBINARY (MAX) not null,
    [PasswordSalt]                     CHAR (32)       null,
    [PasswordModifiedDate]             DATETIME        null,
    [PasswordExpirationDate]           DATETIME        null,
    [InvalidAttemptCount]              INT             null,
	[ResetPasswordEndDate]             DATETIME        null,
	[ResetPasswordKey]                 VARCHAR(72)     null,
	[ResetPasswordSentCount]           SMALLINT        default(0) not null,
	[ResetPasswordInvalidAttemptCount] SMALLINT        default(0) not null,
    [LastLoginDate]                    DATETIME        null,
    [TermsAccepted]                    BIT             not null,
    [DATEnabled]                       BIT             null,
    [Phone]                            VARCHAR (50)    null,
    [BlockedReason]                    NVARCHAR (MAX)  null,
    [CreatedDate]                      DATETIME        null,
    [CreatedBy]                        NVARCHAR (200)  null,
    [ModifiedDate]                     DATETIME        null,
    [ModifiedBy]                       NVARCHAR (200)  null,
    constraint [PK_Login_Log] primary key nonclustered ([Login_LogID] asc)
);
GO

create clustered index [IX_Login_Log]
    on [dbo].[Login_Log]([LoginID] asc);
GO
