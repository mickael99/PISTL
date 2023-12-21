create table [dbo].[LoginImage](
    [LoginImageID] [int] identity(1,1) not null,
    [ExecutionContext] [varchar](50) not null,
    [ImageUrl] [varchar](1024) not null,
    [CreatedDate] [datetime] not null,
    [CreatedBy] [varchar](200) not null,
    constraint [PK_LoginImage] primary key clustered
(
    [LoginImageID] asc
)with (PAD_INDEX = off, STATISTICS_NORECOMPUTE = off, IGNORE_DUP_KEY = off, ALLOW_ROW_LOCKS = on, ALLOW_PAGE_LOCKS = on) on [PRIMARY]
) on [PRIMARY]
go
