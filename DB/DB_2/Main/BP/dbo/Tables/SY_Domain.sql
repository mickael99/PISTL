create table [dbo].[SY_Domain] (
    [DomainID]        INT            not null,
    [DomainName]      NVARCHAR(50)   not null,
    [ParentCompany]   NVARCHAR(50)   null,
    [CreatedDate]     DATETIME       constraint [DF_SY_Domain_CreatedDate] default (getdate()) null,
    [ModifiedDate]    DATETIME       constraint [DF_SY_Domain_ModifiedDate] default (getdate()) not null,
    [ModifiedBy]      NVARCHAR (200) null,
    constraint [PK_SY_Domain] primary key clustered ([DomainID] asc)
);
GO
