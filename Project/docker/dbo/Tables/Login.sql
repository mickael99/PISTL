create table [dbo].[Login] (
    [LoginID]                          INT             identity (1, 1) not null,
    [Email]                            NVARCHAR (200)  null,
    [Name]                             NVARCHAR (50)   not null,
    [Password]                         VARBINARY (MAX) not null,
    [PasswordSalt]                     CHAR(32) 	   null,
    [PasswordModifiedDate]             DATETIME        null,
    [PasswordExpirationDate]           DATETIME        null,
    [InvalidAttemptCount]              INT             null,
	[ResetPasswordEndDate]             DATETIME        null,
	[ResetPasswordKey]                 VARCHAR(72)     null,
	[ResetPasswordSentCount]           SMALLINT        default(0) not null,
	[ResetPasswordInvalidAttemptCount] SMALLINT        default(0) not null,
    [LastLoginDate]                    DATETIME        null,
    [TermsAccepted]                    BIT             constraint [DF_TermsAccepted] default ((0)) not null,
    [DATEnabled]                       BIT             null,
    [Phone]                            VARCHAR (50)    null,
    [BlockedReason]                    NVARCHAR (MAX)  null,
    [CreatedDate]                      DATETIME        constraint [DF_Login_CreatedDate] default (getdate()) null,
    [CreatedBy]                        NVARCHAR (200)  null,
    [ModifiedDate]                     DATETIME        constraint [DF_Login_ModifiedDate] default (getdate()) null,
    [ModifiedBy]                       NVARCHAR (200)  null,
    constraint [PK_Login] primary key clustered ([LoginID] asc)
);
GO


create trigger [dbo].[tr_Login_Delete]
on [dbo].[Login]
after delete
as
begin

	set nocount on

	insert Login_Log
	select 'D', getdate(), *
	from   deleted

end
GO

create trigger [dbo].[tr_Login_Insert]
on [dbo].[Login]
after insert
as
begin

	set nocount on

	insert Login_Log
	select 'I', getdate(), *
	from   inserted

end
GO

create trigger [dbo].[tr_Login_Update]
on [dbo].[Login]
after update
as
begin

	set nocount on

	insert Login_Log
	select 'V', getdate(), *
	from   deleted

	insert Login_Log
	select 'U', getdate(), *
	from   inserted

end
GO
