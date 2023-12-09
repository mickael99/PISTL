create table [dbo].[DomainEnvironment] (
    [DomainEnvironmentID] INT            identity (1, 1) not null,
    [DomainID]            INT            not null,
    [Environment]         INT            not null,
    [BPWebServerID]       INT            not null,
    [BPDatabaseID]        INT            null,
    [EAIDatabaseID]       INT            null,
    [SSRSServerID]        INT            null,
    [TableauServerID]     INT            null,
    [EAIFTPServerID]      INT            null,
    [IsBp5Enabled]        BIT            constraint [DF_DomainEnvironment_IsBp5Enabled] default ((0)) not null,
    [CreatedDate]         DATETIME       constraint [DF_DomainEnvironment_CreatedDate] default (getdate()) null,
    [CreatedBy]           NVARCHAR (200) null,
    [ModifiedDate]        DATETIME       constraint [DF_DomainEnvironment_ModifiedDate] default (getdate()) null,
    [ModifiedBy]          NVARCHAR (200) null,
    constraint [PK_DomainEnvironment] primary key clustered ([DomainEnvironmentID] asc),
    constraint [FK_DomainEnvironment_Database] foreign key ([BPDatabaseID]) references [dbo].[Database] ([DatabaseID]),
    constraint [FK_DomainEnvironment_Database1] foreign key ([EAIDatabaseID]) references [dbo].[Database] ([DatabaseID]),
    constraint [FK_DomainEnvironment_Domain] foreign key ([DomainID]) references [dbo].[Domain] ([DomainID]),
    constraint [FK_DomainEnvironment_Server] foreign key ([BPWebServerID]) references [dbo].[Server] ([ServerID]),
    constraint [FK_DomainEnvironment_Server1] foreign key ([SSRSServerID]) references [dbo].[Server] ([ServerID]),
    constraint [FK_DomainEnvironment_Server4] foreign key ([TableauServerID]) references [dbo].[Server] ([ServerID]),
    constraint [FK_DomainEnvironment_Server5] foreign key ([EAIFTPServerID]) references [dbo].[Server] ([ServerID])
);


GO
create unique nonclustered index [IX_DomainID_Environment]
    on [dbo].[DomainEnvironment]([DomainID] asc, [Environment] asc);

