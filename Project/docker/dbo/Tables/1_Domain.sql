--v11, HFU 2019-06-20, TASK 8287,  Add IsSsoEnabled

create table [dbo].[Domain] (
    [DomainID]           INT             identity (1, 1) not null,
    [Name]               NVARCHAR (50)   not null,
    [Logo]               VARBINARY (MAX) null,
    [Edition]            VARCHAR (MAX)   null,
    [IsSsoEnabled]       BIT             constraint [DF_Domain_IsSSO] default ((0)) not null,
    [Comment]            NVARCHAR (MAX)  null,
    [ParentCompany]      NVARCHAR (50)   null,
    [CreatedDate]        DATETIME        constraint [DF_Domain_CreatedDate] default (getdate()) null,
    [CreatedBy]          NVARCHAR (200)  null,
    [ModifiedDate]       DATETIME        constraint [DF_Domain_ModifiedDate] default (getdate()) null,
    [ModifiedBy]         NVARCHAR (200)  null,
    constraint [PK_Domain] primary key clustered ([DomainID] asc)
);
GO


create trigger [dbo].[tr_Domain_Delete]
on [dbo].[Domain]
after delete
as
begin

	set nocount on

	insert Domain_Log (LogAction, LogDate, DomainID, Name, Logo, Edition, IsSsoEnabled, Comment, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)
	select 'D', getdate(), DomainID, Name, Logo, Edition, IsSsoEnabled, Comment, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy
	from   deleted
end
GO

create trigger [dbo].[tr_Domain_Insert]
on [dbo].[Domain]
after insert
as
begin

	set nocount on

	insert Domain_Log (LogAction, LogDate, DomainID, Name, Logo, Edition, IsSsoEnabled, Comment, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)
	select 'I', getdate(), DomainID, Name, Logo, Edition, IsSsoEnabled, Comment, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy
	from   inserted

end
GO

create trigger [dbo].[tr_Domain_Update]
on [dbo].[Domain]
after update
as
begin

	set nocount on

	insert Domain_Log (LogAction, LogDate, DomainID, Name, Logo, Edition, IsSsoEnabled, Comment, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)
	select 'V', getdate(), DomainID, Name, Logo, Edition, IsSsoEnabled, Comment, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy
	from   deleted

	insert Domain_Log (LogAction, LogDate, DomainID, Name, Logo, Edition, IsSsoEnabled, Comment, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)
	select 'U', getdate(), DomainID, Name, Logo, Edition, IsSsoEnabled, Comment, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy
	from   inserted

end
GO
