create table [dbo].[LoginAuthorizationException] (
    [LoginID] INT not null,
    constraint [PK_LoginAuthorizationException] primary key clustered ([LoginID] asc),
    constraint [FK_LoginAuthorizationException_Login] foreign key ([LoginID]) references [dbo].[Login] ([LoginID])
);

