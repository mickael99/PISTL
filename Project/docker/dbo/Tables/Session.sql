create table [dbo].[Session] (
    [ID]              INT              identity (1, 1) not null,
    [SessionID]       UNIQUEIDENTIFIER not null,
    [Date]            DATETIME         not null,
    [LoginID]         INT              not null,
    [Action]          VARCHAR (50)     not null,
    [UserHostAddress] VARCHAR (50)     null,
    [UserHostName]    VARCHAR (MAX)    null,
    [UserAgent]       VARCHAR (MAX)    null,
    [Data]            VARCHAR (MAX)    null,
    constraint [PK_Session_1] primary key clustered ([ID] asc),
    constraint [FK_Session_Login] foreign key ([LoginID]) references [dbo].[Login] ([LoginID]) on delete cascade on update cascade
);
go

create nonclustered index IXN_Session_LoginID_Action_Date on [dbo].[Session] (LoginID asc, Action asc, Date asc)
go
