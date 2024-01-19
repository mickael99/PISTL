create table [dbo].[Redirect] (
    [RedirectID]     UNIQUEIDENTIFIER not null,
    [ExpirationDate] DATETIME         not null,
    [Action]         VARCHAR (50)     null,
    [Data]           VARCHAR (MAX)    null,
    [Email]          NVARCHAR (200)   not null,
    [DomainID]       INT              not null,
    [Environment]    INT              not null,
    [ProfileID]      VARCHAR (50)     not null,
    constraint [PK_Redirect] primary key clustered ([RedirectID] asc)
);

