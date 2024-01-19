create table [dbo].[PasswordHistory] (
    [PasswordID]    INT             identity (1, 1) not null,
    [LoginID]       INT             not null,
    [Date]          DATETIME        not null,
    [Password]      VARBINARY (MAX) not null,
    [PasswordSalt]  CHAR (32)       null,
    constraint [PK_PasswordHistory_1] primary key clustered ([PasswordID] asc),
    constraint [FK_PasswordHistory_Login] foreign key ([LoginID]) references [dbo].[Login] ([LoginID]) on delete cascade on update cascade
);

