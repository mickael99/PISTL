create table [dbo].[Database] (
    [DatabaseID]   INT            not null,
    [Name]         VARCHAR (20)   not null,
    [UserName]     VARCHAR (20)   not null,
    [Password]     NVARCHAR (MAX) not null,
    [Context]      INT            null,
    [ServerID]     INT            not null,
    [CreatedDate]  DATETIME       constraint [DF_Database_CreatedDate] default (getdate()) null,
    [CreatedBy]    NVARCHAR (200) null,
    [ModifiedDate] DATETIME       constraint [DF_Database_ModifiedDate] default (getdate()) null,
    [ModifiedBy]   NVARCHAR (200) null,
    constraint [PK_Database] primary key clustered ([DatabaseID] asc),
    constraint [FK_Database_Server] foreign key ([ServerID]) references [dbo].[Server] ([ServerID])
);
GO

create unique nonclustered index [IX_Database_NameServerID]
    on [dbo].[Database]([Name] asc, [ServerID] asc);
GO

