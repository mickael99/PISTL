create table [dbo].[SingleSignOnSaml2ContactPerson] (
    [ContactPersonID] INT               identity (1, 1) not null,
    [Company]         VARCHAR (100)     not null,
    [Type]            INT               not null,
    [EmailAddresses]  VARCHAR (50)      not null,
    [GivenName]       VARCHAR (50)      not null,
    [Surname]         VARCHAR (50)      not null,
    [ModifiedBy]      NVARCHAR (200)    not null,
    [ModifiedDate]    DATETIME          not null,
    primary key clustered ([ContactPersonID] asc)
);

