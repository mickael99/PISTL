create table [dbo].[Domain_Log] (
    [Domain_LogID]  INT             identity (1, 1) not null,
    [LogAction]     CHAR (1)        not null,
    [LogDate]       DATETIME        not null,
    [DomainID]      INT             not null,
    [Name]          NVARCHAR (50)   not null,
    [Logo]          VARBINARY (MAX) null,
    [Edition]       VARCHAR (MAX)   null,
    [IsSsoEnabled]  BIT             not null,
    [Comment]       NVARCHAR (MAX)  null,
    [ParentCompany] NVARCHAR (50)   null,
    [CreatedDate]   DATETIME        null,
    [CreatedBy]     NVARCHAR (200)  null,
    [ModifiedDate]  DATETIME        null,
    [ModifiedBy]    NVARCHAR (200)  null,
    constraint [PK_Domain_Log] primary key nonclustered ([Domain_LogID] asc)
);
GO

create clustered index [IX_Domain_Log]
    on [dbo].[Domain_Log]([DomainID] asc);
GO
