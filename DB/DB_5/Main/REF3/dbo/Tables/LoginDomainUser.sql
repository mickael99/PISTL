
create table [dbo].[LoginDomainUser] (
    [LoginID]                INT             not null,
    [DomainID]               INT             not null,
    [UserID]                 VARCHAR (50)    not null,
    [Environment]            INT             constraint [DF_LoginDomainUser_Environment] default ((0)) not null,
    [UserName]               NVARCHAR (200)  null,
    [UserActive]             BIT             not null,
    [LoginEnabled]           BIT             constraint [DF_LoginDomainUser_Active] default ((1)) not null,
    [LoginTypeId]            INT             null,
    [AnalyticsEnabled]       BIT             null,
    [IsLight]                BIT             null,
    [SysAdmin]               BIT             constraint [DF_LoginDomainUser_SysAdmin] default ((0)) not null,
    [SysAdminStartDate]      DATETIME        null,
    [SysAdminEndDate]        DATETIME        null,
    [DomainLastLoginDate]    DATETIME        null,
    [Comment]                NVARCHAR (2000) null,
    [CreatedDate]            DATETIME        constraint [DF_LoginDomainUser_CreatedDate] default (getdate()) null,
    [CreatedBy]              NVARCHAR (200)  null,
    [ModifiedDate]           DATETIME        constraint [DF_LoginDomainUser_ModifiedDate] default (getdate()) null,
    [ModifiedBy]             NVARCHAR (200)  null,
    constraint [PK_LoginDomainUser] primary key clustered ([LoginID] asc, [DomainID] asc, [UserID] asc, [Environment] asc),
    constraint [FK_LoginDomainUser_Domain] foreign key ([DomainID]) references [dbo].[Domain] ([DomainID]),
    constraint [FK_LoginDomainUser_Login] foreign key ([LoginID]) references [dbo].[Login] ([LoginID])
);
GO


create trigger [dbo].[tr_LoginDomainUser_Delete]
on [dbo].[LoginDomainUser]
after delete
		--v10, HF 2019-03-14, Task 7358: Logging changes to LoginDomainUser
as
begin

	set nocount on

	insert LoginDomainUser_Log
	select 'D', getdate(), *
	from   deleted

end
GO

create trigger [dbo].[tr_LoginDomainUser_Insert]
on [dbo].[LoginDomainUser]
after insert
		--v10, HF 2019-03-14, Task 7358: Logging changes to LoginDomainUser
as
begin

	set nocount on

	insert LoginDomainUser_Log
	select 'I', getdate(), *
	from   inserted

end
GO

create trigger [dbo].[tr_LoginDomainUser_Update]
on [dbo].[LoginDomainUser]
after update
as
begin

	set nocount on

	insert LoginDomainUser_Log
	select 'U', getdate(), *
	from   inserted

end
GO
