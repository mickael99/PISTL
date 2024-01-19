create table [dbo].[Server] (
    [ServerID]           INT                      not null,
    [Address]            VARCHAR (1024)           not null,
    [Bp5Address]         VARCHAR (1024)           null,
    [Bp5FrontAddress]    VARCHAR (1024)           null,
    [Name]               VARCHAR (50)             not null,
    [Context]            INT                      null,
    [Type]               VARCHAR (50)             null,
    [SubscriptionID]     UNIQUEIDENTIFIER         null,
    [ResourceGroup]      VARCHAR(200)             null,
    [CreatedDate]        DATETIME                 constraint [DF_Server_CreatedDate] default (getdate()) null,
    [CreatedBy]          NVARCHAR (200)           null,
    [ModifiedDate]       DATETIME                 constraint [DF_Server_ModifiedDate] default (getdate()) null,
    [ModifiedBy]         NVARCHAR (200)           null,
    constraint [PK_Server] primary key clustered ([ServerID] asc)
);

