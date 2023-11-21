create table [dbo].[LoginNews](
    [LoginNewsID] [int] identity(1,1) not null,
    [ImageUrl] [varchar](1024) not null,
    [LinkUrl] [varchar](1024) null,
    [Title] [nvarchar](200) null,
    [IsActive] [bit] not null default(0),
    [CreatedDate] DATETIME not null,
    [CreatedBy] [varchar](200) not null,
    [ModifiedDate] DATETIME null,
    [ModifiedBy] [varchar](200) null,
    constraint [PK_LoginNews] primary key clustered
(
    [LoginNewsID] asc
)with (PAD_INDEX = off, STATISTICS_NORECOMPUTE = off, IGNORE_DUP_KEY = off, ALLOW_ROW_LOCKS = on, ALLOW_PAGE_LOCKS = on) on [PRIMARY]
) on [PRIMARY]
