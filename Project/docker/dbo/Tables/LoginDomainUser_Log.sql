--v10, HF 2019-03-14, Task 7358: Logging changes to LoginDomainUser

create table [dbo].[LoginDomainUser_Log] (
    [LoginDomainUser_LogID]  INT             identity (1, 1) not null,
    [LogAction]              CHAR (1)        not null,
    [LogDate]                DATETIME        not null,
    [LoginID]                INT             not null,
    [DomainID]               INT             not null,
    [UserID]                 VARCHAR (50)    not null,
    [Environment]            INT             not null,
    [UserName]               NVARCHAR (200)  null,
    [UserActive]             BIT             not null,
    [LoginEnabled]           BIT             not null,
    [LoginTypeId]            INT             null,
    [AnalyticsEnabled]       BIT             null,
    [IsLight]                BIT             null,
    [SysAdmin]               BIT             not null,
    [SysAdminStartDate]      DATETIME        null,
    [SysAdminEndDate]        DATETIME        null,
    [DomainLastLoginDate]    DATETIME        null,
    [Comment]                NVARCHAR (2000) null,
    [CreatedDate]            DATETIME        null,
    [CreatedBy]              NVARCHAR (200)  null,
    [ModifiedDate]           DATETIME        null,
    [ModifiedBy]             NVARCHAR (200)  null,
);
GO

create clustered index [IX_LoginDomainUser_Log]
    on [dbo].[LoginDomainUser_Log]([DomainID] asc);
GO
